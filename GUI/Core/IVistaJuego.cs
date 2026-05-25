using ENTITY;
using System;

namespace GUI
{
    public interface IVistaJuego
    {
        event EventHandler SaldoActualizado;
        void InicializarJuego(Usuario usuario);
    }
}
