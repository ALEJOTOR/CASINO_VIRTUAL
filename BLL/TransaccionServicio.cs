using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL
{
    public class TransaccionServicio
    {
        private readonly TransaccionRepositorio _transRepo = new TransaccionRepositorio();
        private readonly PartidaRepositorio _partidaRepo = new PartidaRepositorio();
        private readonly JuegoRepositorio _juegoRepo = new JuegoRepositorio();
        private readonly UsuarioServicio _usuarioSvc = new UsuarioServicio();
        private readonly LogServicio _logSvc = new LogServicio();

        public string RealizarDeposito(int idUsuario, decimal monto)
        {
            if (monto <= 0) return "El monto debe ser mayor a 0.";
            string resultado = _transRepo.RegistrarDepositoConSaldo(idUsuario, monto);
            if (resultado.Contains("correctamente"))
                _logSvc.Registrar("deposito", "INFO", "BILLETERA", $"Deposito ${monto}", idUsuario);
            return resultado;
        }

        public string RealizarRetiro(int idUsuario, decimal monto)
        {
            string resultado = _usuarioSvc.RetirarSaldo(idUsuario, monto);
            if (resultado.Contains("correctamente"))
                _logSvc.Registrar("retiro", "INFO", "BILLETERA", $"Retiro ${monto}", idUsuario);
            return resultado;
        }

        public IList<Transaccion> ObtenerPorUsuario(int idUsuario)
        {
            return _transRepo.ObtenerPorUsuario(idUsuario);
        }

        public IList<Transaccion> ObtenerTodas()
        {
            return _transRepo.Consultar();
        }

        public IList<TransaccionDisplayDto> ObtenerTodasConNombres()
        {
            IList<Transaccion> lista = _transRepo.Consultar();
            IList<Usuario> usuarios = new UsuarioServicio().ObtenerTodos();
            Dictionary<int, string> mapaUsuarios = usuarios.ToDictionary(u => u.IdUsuario, u => $"{u.Nombre1} {u.Apellido1}");

            return lista.Select(t => new TransaccionDisplayDto
            {
                IdTransaccion = t.IdTransaccion,
                Usuario = mapaUsuarios.ContainsKey(t.IdUsuario) ? mapaUsuarios[t.IdUsuario] : $"Usuario {t.IdUsuario}",
                Tipo = char.ToUpper(t.Tipo[0]) + t.Tipo.Substring(1),
                Monto = t.Monto,
                Fecha = t.Fecha,
                Descripcion = t.Descripcion ?? ""
            }).ToList();
        }

        public IList<MovimientoResumen> ObtenerMovimientosResumen(
            int idUsuario, string categoria, string filtro,
            DateTime desde, DateTime hasta)
        {
            List<MovimientoResumen> resultado = new List<MovimientoResumen>();
            Dictionary<int, string> nombresJuegos = ObtenerMapaNombresJuegos();

            if (categoria == "Todos" || categoria == "Depositos")
            {
                foreach (Transaccion t in _transRepo.ObtenerPorUsuario(idUsuario))
                {
                    if (categoria == "Depositos" && t.Tipo != "deposito" && t.Tipo != "retiro")
                        continue;

                    bool entrada = t.Tipo == "deposito" || t.Tipo == "ganancia";
                    MovimientoResumen m = new MovimientoResumen
                    {
                        Titulo = ResolverDescripcionMovimiento(t.Descripcion ?? t.Tipo, nombresJuegos),
                        Tipo = t.Tipo,
                        Fecha = t.Fecha,
                        Monto = entrada ? t.Monto : -t.Monto
                    };

                    if (m.Fecha >= desde && m.Fecha <= hasta && CumpleFiltro(categoria, filtro, m))
                        resultado.Add(m);
                }
            }
            else
            {
                foreach (HistorialPartida p in _partidaRepo.ObtenerHistorialPorUsuario(idUsuario))
                {
                    string juego = ResolverNombreJuego(p.NombreJuego, nombresJuegos);

                    MovimientoResumen apuesta = new MovimientoResumen
                    {
                        Titulo = $"{juego}: monto apostado",
                        Tipo = "perdida",
                        Fecha = p.Fecha,
                        Monto = -p.Apuesta
                    };

                    if (apuesta.Fecha >= desde && apuesta.Fecha <= hasta &&
                        CumpleFiltro(categoria, filtro, apuesta))
                        resultado.Add(apuesta);

                    if (p.Ganancia > 0)
                    {
                        MovimientoResumen ganancia = new MovimientoResumen
                        {
                            Titulo = $"{juego}: resultado de la apuesta",
                            Tipo = "ganancia",
                            Fecha = p.Fecha,
                            Monto = p.Ganancia
                        };

                        if (ganancia.Fecha >= desde && ganancia.Fecha <= hasta &&
                            CumpleFiltro(categoria, filtro, ganancia))
                            resultado.Add(ganancia);
                    }
                }
            }

            return resultado.OrderByDescending(m => m.Fecha).ToList();
        }

        /// <summary>
        /// Obtiene movimientos paginados con filtros de categoría y fecha.
        /// Combina transacciones simples (depósitos/retiros) con partidas (apuestas/ganancias).
        /// </summary>
        /// <param name="idUsuario">ID del usuario</param>
        /// <param name="pageNumber">Número de página (1-based)</param>
        /// <param name="pageSize">Cantidad de registros por página</param>
        /// <param name="categoria">Filtro: null/"Todos", "Depositos", "Apuestas"</param>
        /// <param name="fechaDesde">Filtro de fecha desde (nullable)</param>
        /// <param name="fechaHasta">Filtro de fecha hasta (nullable)</param>
        /// <returns>Tupla (lista de movimientos, total de registros con filtro)</returns>
        public async Task<(IList<MovimientoResumen> items, int totalCount)> ObtenerMovimientosPaginadosAsync(
            int idUsuario, int pageNumber, int pageSize,
            string categoria = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            return await Task.Run(() =>
            {
                // Normalizar categoría
                categoria = string.IsNullOrEmpty(categoria) ? "Todos" : categoria;
                
                // Obtener los datos paginados del repositorio
                var (transacciones, totalTransacciones) = _transRepo.ObtenerPaginado(
                    idUsuario, pageNumber, pageSize, categoria, fechaDesde, fechaHasta);

                Dictionary<int, string> nombresJuegos = ObtenerMapaNombresJuegos();
                List<MovimientoResumen> resultado = new List<MovimientoResumen>();

                // Convertir transacciones a MovimientoResumen
                foreach (Transaccion t in transacciones)
                {
                    bool entrada = t.Tipo == "deposito" || t.Tipo == "ganancia";
                    MovimientoResumen m = new MovimientoResumen
                    {
                        Titulo = ResolverDescripcionMovimiento(t.Descripcion ?? t.Tipo, nombresJuegos),
                        Tipo = t.Tipo,
                        Fecha = t.Fecha,
                        Monto = entrada ? t.Monto : -t.Monto
                    };
                    resultado.Add(m);
                }

                // Si se pide "Apuestas", también cargar partidas (con lógica similar al método antiguo)
                if (categoria == "Apuestas")
                {
                    foreach (HistorialPartida p in _partidaRepo.ObtenerHistorialPorUsuario(idUsuario))
                    {
                        if (fechaDesde.HasValue && p.Fecha < fechaDesde) continue;
                        if (fechaHasta.HasValue && p.Fecha > fechaHasta) continue;

                        string juego = ResolverNombreJuego(p.NombreJuego, nombresJuegos);

                        MovimientoResumen apuesta = new MovimientoResumen
                        {
                            Titulo = string.Format("{0}: monto apostado", juego),
                            Tipo = "perdida",
                            Fecha = p.Fecha,
                            Monto = -p.Apuesta
                        };
                        resultado.Add(apuesta);

                        if (p.Ganancia > 0)
                        {
                            MovimientoResumen ganancia = new MovimientoResumen
                            {
                                Titulo = string.Format("{0}: resultado de la apuesta", juego),
                                Tipo = "ganancia",
                                Fecha = p.Fecha,
                                Monto = p.Ganancia
                            };
                            resultado.Add(ganancia);
                        }
                    }

                    // Para Apuestas, recalcular total paginado después de agregar partidas
                    resultado = resultado.OrderByDescending(m => m.Fecha).ToList();
                    int totalPaginado = resultado.Count;
                    int skipAmount = (pageNumber - 1) * pageSize;
                    resultado = resultado.Skip(skipAmount).Take(pageSize).ToList();

                    return (resultado, totalPaginado);
                }

                return (resultado, totalTransacciones);
            });
        }

        private bool CumpleFiltro(string categoria, string filtro, MovimientoResumen m)
        {
            if (filtro == "Todos" || filtro == "Todas") return true;

            if (categoria == "Apuestas")
            {
                if (filtro == "Montos apostados") return m.Tipo == "perdida";
                if (filtro == "Premios ganados") return m.Tipo == "ganancia";
            }

            if (categoria == "Depositos")
            {
                if (filtro == "Depositos") return m.Tipo == "deposito";
                if (filtro == "Retiros") return m.Tipo == "retiro";
            }

            if (filtro == "Depositos") return m.Tipo == "deposito";
            if (filtro == "Retiros") return m.Tipo == "retiro";
            if (filtro == "Apuestas") return m.Tipo == "perdida";
            if (filtro == "Ganancias") return m.Tipo == "ganancia";

            return true;
        }

        private string ResolverDescripcionMovimiento(string descripcion, Dictionary<int, string> nombresJuegos)
        {
            if (string.IsNullOrWhiteSpace(descripcion)) return "Movimiento";

            string texto = descripcion.Trim();
            string lower = texto.ToLower();
            int indiceJuego = lower.IndexOf("juego");
            if (indiceJuego < 0) return texto;

            int inicioNumero = indiceJuego + "juego".Length;
            while (inicioNumero < texto.Length && texto[inicioNumero] == ' ')
                inicioNumero++;

            int finNumero = inicioNumero;
            while (finNumero < texto.Length && char.IsDigit(texto[finNumero]))
                finNumero++;

            if (finNumero == inicioNumero) return texto;

            string numero = texto.Substring(inicioNumero, finNumero - inicioNumero);
            if (!int.TryParse(numero, out int idJuego)) return texto;

            string nombre = ResolverNombreJuego(numero, nombresJuegos);
            if (nombre == numero || nombre.ToLower().Contains("juego"))
                nombre = NombreJuegoConocido(idJuego);

            string reemplazo = texto.Substring(indiceJuego, finNumero - indiceJuego);
            return texto.Replace(reemplazo, nombre);
        }

        private Dictionary<int, string> ObtenerMapaNombresJuegos()
        {
            try
            {
                return _juegoRepo.Consultar()
                    .GroupBy(j => j.IdJuego)
                    .ToDictionary(g => g.Key, g => g.First().Nombre);
            }
            catch
            {
                return new Dictionary<int, string>();
            }
        }

        private string ResolverNombreJuego(string valor, Dictionary<int, string> nombresJuegos)
        {
            if (string.IsNullOrWhiteSpace(valor)) return "Juego";

            string texto = valor.Trim();
            string lower = texto.ToLower();
            int id = 0;

            if (lower.Contains("juego"))
            {
                string digitos = new string(texto.Where(char.IsDigit).ToArray());
                if (int.TryParse(digitos, out id) && nombresJuegos.ContainsKey(id))
                    return nombresJuegos[id];
            }

            if (int.TryParse(texto, out id) && nombresJuegos.ContainsKey(id))
                return nombresJuegos[id];

            return texto;
        }

        private string NombreJuegoConocido(int idJuego)
        {
            if (idJuego == 1) return "Minas";
            if (idJuego == 2) return "Ruleta";
            if (idJuego == 3) return "Tragamonedas";
            if (idJuego == 4) return "Tragamonedas";
            return "Juego";
        }
    }
}
