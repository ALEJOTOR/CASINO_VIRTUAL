using System;
using System.Collections.Generic;
using System.IO;
using ENTITY;

namespace DAL
{
    // ══════════════════════════════════════════════════════════════
    // JUEGO DAL
    // Formato: id|nombre|descripcion|estado
    // ══════════════════════════════════════════════════════════════
    public class JuegoDAL
    {
        public List<Juego> ConsultarTodos()
        {
            var lista = new List<Juego>();
            if (!File.Exists(RutaArchivos.Juegos)) return lista;

            using (StreamReader sr = new StreamReader(RutaArchivos.Juegos))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linea)) continue;
                    string[] c = linea.Split('|');
                    lista.Add(new Juego
                    {
                        IdJuego     = int.Parse(c[0]),
                        Nombre      = c[1],
                        Descripcion = c[2],
                        Estado      = c[3]
                    });
                }
            }
            return lista;
        }

        private int SiguienteId()
        {
            var lista = ConsultarTodos();
            return lista.Count == 0 ? 1 : lista[lista.Count - 1].IdJuego + 1;
        }

        public bool Insertar(Juego j)
        {
            RutaArchivos.CrearCarpeta();
            j.IdJuego = SiguienteId();
            using (StreamWriter sw = new StreamWriter(RutaArchivos.Juegos, append: true))
                sw.WriteLine($"{j.IdJuego}|{j.Nombre}|{j.Descripcion}|{j.Estado}");
            return true;
        }

        public void InicializarDatos()
        {
            if (File.Exists(RutaArchivos.Juegos) && new FileInfo(RutaArchivos.Juegos).Length > 0)
                return;
            Insertar(new Juego { Nombre = "Minas", Descripcion = "Juego tipo minas con apuestas", Estado = "activo" });
        }
    }

    // ══════════════════════════════════════════════════════════════
    // ESTADO PARTIDA DAL
    // Formato: id|nombre_estado|descripcion
    // ══════════════════════════════════════════════════════════════
    public class EstadoPartidaDAL
    {
        public List<EstadoPartida> ConsultarTodos()
        {
            var lista = new List<EstadoPartida>();
            if (!File.Exists(RutaArchivos.EstadoPartidas)) return lista;

            using (StreamReader sr = new StreamReader(RutaArchivos.EstadoPartidas))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linea)) continue;
                    string[] c = linea.Split('|');
                    lista.Add(new EstadoPartida
                    {
                        IdEstado     = int.Parse(c[0]),
                        NombreEstado = c[1],
                        Descripcion  = c[2]
                    });
                }
            }
            return lista;
        }

        private int SiguienteId()
        {
            var lista = ConsultarTodos();
            return lista.Count == 0 ? 1 : lista[lista.Count - 1].IdEstado + 1;
        }

        public bool Insertar(EstadoPartida e)
        {
            RutaArchivos.CrearCarpeta();
            e.IdEstado = SiguienteId();
            using (StreamWriter sw = new StreamWriter(RutaArchivos.EstadoPartidas, append: true))
                sw.WriteLine($"{e.IdEstado}|{e.NombreEstado}|{e.Descripcion}");
            return true;
        }

        public void InicializarDatos()
        {
            if (File.Exists(RutaArchivos.EstadoPartidas) && new FileInfo(RutaArchivos.EstadoPartidas).Length > 0)
                return;
            Insertar(new EstadoPartida { NombreEstado = "en curso",  Descripcion = "Partida activa" });
            Insertar(new EstadoPartida { NombreEstado = "ganada",    Descripcion = "El usuario ganó" });
            Insertar(new EstadoPartida { NombreEstado = "perdida",   Descripcion = "El usuario perdió" });
            Insertar(new EstadoPartida { NombreEstado = "cancelada", Descripcion = "Partida cancelada" });
        }
    }

    // ══════════════════════════════════════════════════════════════
    // PARTIDA DAL
    // Formato: id|idUsuario|idJuego|idEstado|fecha|apuesta|ganancia|resultado
    // ══════════════════════════════════════════════════════════════
    public class PartidaDAL
    {
        public List<Partida> ConsultarTodos()
        {
            var lista = new List<Partida>();
            if (!File.Exists(RutaArchivos.Partidas)) return lista;

            using (StreamReader sr = new StreamReader(RutaArchivos.Partidas))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linea)) continue;
                    string[] c = linea.Split('|');
                    lista.Add(new Partida
                    {
                        IdPartida  = int.Parse(c[0]),
                        IdUsuario  = int.Parse(c[1]),
                        IdJuego    = int.Parse(c[2]),
                        IdEstado   = int.Parse(c[3]),
                        Fecha      = DateTime.Parse(c[4]),
                        Apuesta    = decimal.Parse(c[5]),
                        Ganancia   = decimal.Parse(c[6]),
                        Resultado  = c[7]
                    });
                }
            }
            return lista;
        }

        public List<Partida> ConsultarPorUsuario(int idUsuario)
        {
            var lista = new List<Partida>();
            foreach (var p in ConsultarTodos())
                if (p.IdUsuario == idUsuario) lista.Add(p);
            return lista;
        }

        private int SiguienteId()
        {
            var lista = ConsultarTodos();
            return lista.Count == 0 ? 1 : lista[lista.Count - 1].IdPartida + 1;
        }

        public bool Insertar(Partida p)
        {
            RutaArchivos.CrearCarpeta();
            p.IdPartida = SiguienteId();
            p.Fecha     = DateTime.Now;

            using (StreamWriter sw = new StreamWriter(RutaArchivos.Partidas, append: true))
                sw.WriteLine(string.Join("|",
                    p.IdPartida, p.IdUsuario, p.IdJuego, p.IdEstado,
                    p.Fecha.ToString("yyyy-MM-dd HH:mm:ss"),
                    p.Apuesta.ToString("F2"),
                    p.Ganancia.ToString("F2"),
                    p.Resultado ?? ""));
            return true;
        }
    }

    // ══════════════════════════════════════════════════════════════
    // TRANSACCION DAL
    // Formato: id|idUsuario|tipo|monto|fecha|descripcion
    // ══════════════════════════════════════════════════════════════
    public class TransaccionDAL
    {
        public List<Transaccion> ConsultarPorUsuario(int idUsuario)
        {
            var lista = new List<Transaccion>();
            if (!File.Exists(RutaArchivos.Transacciones)) return lista;

            using (StreamReader sr = new StreamReader(RutaArchivos.Transacciones))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(linea)) continue;
                    string[] c = linea.Split('|');
                    if (int.Parse(c[1]) != idUsuario) continue;
                    lista.Add(new Transaccion
                    {
                        IdTransaccion = int.Parse(c[0]),
                        IdUsuario     = int.Parse(c[1]),
                        Tipo          = c[2],
                        Monto         = decimal.Parse(c[3]),
                        Fecha         = DateTime.Parse(c[4]),
                        Descripcion   = c[5]
                    });
                }
            }
            return lista;
        }

        private int SiguienteId()
        {
            int max = 0;
            if (!File.Exists(RutaArchivos.Transacciones)) return 1;
            using (StreamReader sr = new StreamReader(RutaArchivos.Transacciones))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                    if (!string.IsNullOrWhiteSpace(linea))
                        max = int.Parse(linea.Split('|')[0]);
            }
            return max + 1;
        }

        public bool Insertar(Transaccion t)
        {
            RutaArchivos.CrearCarpeta();
            t.IdTransaccion = SiguienteId();
            t.Fecha         = DateTime.Now;

            using (StreamWriter sw = new StreamWriter(RutaArchivos.Transacciones, append: true))
                sw.WriteLine(string.Join("|",
                    t.IdTransaccion, t.IdUsuario, t.Tipo,
                    t.Monto.ToString("F2"),
                    t.Fecha.ToString("yyyy-MM-dd HH:mm:ss"),
                    t.Descripcion ?? ""));
            return true;
        }
    }
}
