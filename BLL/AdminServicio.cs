using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class AdminServicio
    {
        private readonly AdminRepositorio _adminRepo = new AdminRepositorio();

        public ResumenAdmin ObtenerResumenGeneral()
        {
            return _adminRepo.ObtenerResumenGeneral();
        }

        public IList<EstadisticasUsuario> ObtenerTopJugadores(int cantidad)
        {
            return _adminRepo.ObtenerTopJugadores(cantidad);
        }

        public Dictionary<string, int> ObtenerPartidasPorJuego()
        {
            return _adminRepo.ObtenerPartidasPorJuego();
        }

        public Dictionary<DateTime, decimal> ObtenerIngresosPorDia(int dias)
        {
            return _adminRepo.ObtenerIngresosPorDia(dias);
        }

        public Dictionary<DateTime, decimal> ObtenerGananciaCasaPorDia(int dias)
        {
            return _adminRepo.ObtenerGananciaCasaPorDia(dias);
        }

        public IList<Partida> ObtenerPartidasRecientes(int cantidad)
        {
            return _adminRepo.ObtenerPartidasRecientes(cantidad);
        }

        public IList<(int IdUsuario, decimal TotalDepositado, int NumDepositos)> ObtenerTopDepositantes(int cantidad)
        {
            return _adminRepo.ObtenerTopDepositantes(cantidad);
        }

        public decimal ObtenerTotalTransacciones(string tipo)
        {
            return _adminRepo.ObtenerTotalTransacciones(tipo);
        }

        public IList<(string Juego, int Partidas, decimal TotalApostado, decimal GananciaCasa, decimal Margen)> ObtenerRentabilidadPorJuego()
        {
            return _adminRepo.ObtenerRentabilidadPorJuego();
        }

        public Dictionary<string, (decimal Depositos, decimal Retiros)> ObtenerMovimientosPorMes(int meses)
        {
            return _adminRepo.ObtenerMovimientosPorMes(meses);
        }

        public string GenerarReporteUsuarios()
    {
        ResumenAdmin resumen = ObtenerResumenGeneral();
        IList<EstadisticasUsuario> top = ObtenerTopJugadores(10);
        UsuarioServicio usuarioSvc = new UsuarioServicio();

        int totalUsuarios = resumen.TotalUsuarios;
        int activos = resumen.UsuariosActivos;

        string reporte = $"REPORTE DE USUARIOS\n" +
                         $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}\n" +
                         $"──────────────────────────────────────\n" +
                         $"Total usuarios:        {totalUsuarios}\n" +
                         $"Usuarios activos:      {activos}\n" +
                         $"Inactivos/bloqueados:  {totalUsuarios - activos}\n\n" +
                         $"TOP 10 JUGADORES POR ACTIVIDAD\n" +
                         $"──────────────────────────────\n";

        int i = 1;
        foreach (var jug in top)
        {
            reporte += $"{i,2}. {jug.NombreUsuario,-25} Partidas: {jug.TotalPartidas,5}  " +
                       $"Apostado: ${jug.TotalApostado,8:N2}  Ganado: ${jug.TotalGanado,8:N2}\n";
            i++;
        }

        return reporte;
    }

    public string GenerarReporteFinanciero(DateTime desde, DateTime hasta)
    {
        decimal totalDepositos = ObtenerTotalTransacciones("deposito");
        decimal totalRetiros = ObtenerTotalTransacciones("retiro");
        decimal totalBonos = ObtenerTotalTransacciones("bono");
        decimal gananciaCasa = ObtenerResumenGeneral().GananciaCasaTotal;

        var movimientos = ObtenerMovimientosPorMes(6);

        string reporte = $"REPORTE FINANCIERO\n" +
                         $"Período: {desde:dd/MM/yyyy} - {hasta:dd/MM/yyyy}\n" +
                         $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}\n" +
                         $"──────────────────────────────────────\n" +
                         $"Total depósitos:       ${totalDepositos,12:N2}\n" +
                         $"Total retiros:         ${totalRetiros,12:N2}\n" +
                         $"Balance:               ${totalDepositos - totalRetiros,12:N2}\n" +
                         $"Total bonos aplicados: ${totalBonos,12:N2}\n" +
                         $"Ganancia de la casa:   ${gananciaCasa,12:N2}\n\n" +
                         $"MOVIMIENTOS POR MES (últimos 6)\n" +
                         $"──────────────────────────────\n";

        foreach (var kv in movimientos)
        {
            reporte += $"{kv.Key,-8}  Depósitos: ${kv.Value.Depositos,10:N2}  " +
                       $"Retiros: ${kv.Value.Retiros,10:N2}  " +
                       $"Neto: ${kv.Value.Depositos - kv.Value.Retiros,10:N2}\n";
        }

        return reporte;
    }

    public string GenerarReportePartidas(DateTime desde, DateTime hasta)
    {
        PartidaServicio partidaSvc = new PartidaServicio();
        IList<PartidaDisplayDto> partidas = partidaSvc.ObtenerFiltradasConNombres(desde, hasta, null, null);
        var rentabilidad = ObtenerRentabilidadPorJuego();

        int total = partidas.Count;
        int ganadas = partidas.Count(p => p.Estado.Equals("ganada", StringComparison.OrdinalIgnoreCase));
        int perdidas = partidas.Count(p => p.Estado.Equals("perdida", StringComparison.OrdinalIgnoreCase));
        decimal totalApostado = partidas.Sum(p => p.Apuesta);
        decimal totalGanado = partidas.Sum(p => p.Ganancia);

        string reporte = $"REPORTE DE PARTIDAS\n" +
                         $"Período: {desde:dd/MM/yyyy} - {hasta:dd/MM/yyyy}\n" +
                         $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}\n" +
                         $"──────────────────────────────────────\n" +
                         $"Total partidas:        {total,6}\n" +
                         $"Ganadas:               {ganadas,6}  ({(total > 0 ? (ganadas * 100m / total) : 0),5:F1}%)\n" +
                         $"Perdidas:              {perdidas,6}  ({(total > 0 ? (perdidas * 100m / total) : 0),5:F1}%)\n" +
                         $"Total apostado:        ${totalApostado,10:N2}\n" +
                         $"Total ganado:          ${totalGanado,10:N2}\n" +
                         $"Ganancia neta:         ${totalGanado - totalApostado,10:N2}\n\n" +
                         $"RENTABILIDAD POR JUEGO\n" +
                         $"──────────────────────────────\n";

        foreach (var r in rentabilidad)
        {
            reporte += $"{r.Juego,-20}  Partidas: {r.Partidas,5}  " +
                       $"Ganancia Casa: ${r.GananciaCasa,8:N2}  Margen: {r.Margen,6:F2}%\n";
        }

        reporte += $"\nTOP JUGADORES\n";
        reporte += $"──────────────────────────────\n";
        var top = ObtenerTopJugadores(5);
        int idx = 1;
        foreach (var j in top)
        {
            reporte += $"{idx,2}. {j.NombreUsuario,-20}  Partidas: {j.TotalPartidas,4}  " +
                       $"Apostado: ${j.TotalApostado,8:N2}\n";
            idx++;
        }

        return reporte;
    }

    public string GenerarReporteBonos()
    {
        BonoServicio bonoSvc = new BonoServicio();
        IList<Bono> bonos = bonoSvc.ObtenerTodos();
        IList<UsuarioBono> aplicados = bonoSvc.ObtenerTodosParaAdmin();

        int activos = bonos.Count(b => b.Activo);
        int totalAplicados = aplicados.Count;
        decimal montoTotal = aplicados.Sum(b => b.MontoAplicado);

        var porTipo = bonos.GroupBy(b => b.Tipo)
            .Select(g => new { Tipo = g.Key, Cantidad = g.Count(), Monto = aplicados.Where(a => a.TipoBono == g.Key).Sum(a => a.MontoAplicado) })
            .ToList();

        string reporte = $"REPORTE DE BONOS\n" +
                         $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}\n" +
                         $"──────────────────────────────────────\n" +
                         $"Bonos en catálogo:     {bonos.Count}\n" +
                         $"Bonos activos:         {activos}\n" +
                         $"Bonos inactivos:       {bonos.Count - activos}\n" +
                         $"Bonos aplicados:       {totalAplicados}\n" +
                         $"Monto total bonificado: ${montoTotal:N2}\n\n" +
                         $"BONOS POR TIPO\n" +
                         $"──────────────────────────────\n";

        foreach (var t in porTipo)
        {
            reporte += $"{t.Tipo,-20}  Cantidad: {t.Cantidad,3}  " +
                       $"Monto bonificado: ${t.Monto,10:N2}\n";
        }

        reporte += $"\nCATÁLOGO DE BONOS\n";
        reporte += $"──────────────────────────────\n";
        foreach (var b in bonos)
        {
            reporte += $"{b.Nombre,-25}  Tipo: {b.Tipo,-15}  " +
                       $"Valor: ${b.Valor,8:N2}  {(b.Activo ? "ACTIVO" : "INACTIVO")}\n";
        }

        return reporte;
    }
}
}
