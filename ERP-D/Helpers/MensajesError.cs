using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_D.Helpers
{
    public class MensajesError
    {

        public const string NoValido = "Correo electronico no valido.";
        public const string UserOPassInvalidos = "Usuario o contraseña no son válidos.";
        public const string ExisteGerenciaGeneral = "Ya existe una gerencia general.";
        public const string ExisteEmpresa = "El nombre de la empresa se encuentra en uso";
        public const string ExisteGerencia = "El nombre de la gerencia se encuentra en uso";
        public const string ExistePosicion = "El nombre de la posicion se encuentra en uso";
        public const string montoSuperado = "Este centro de costo no admite mas gastos";
        public const string ExisteCentroDeCosto= "El nombre del centro de costo se encuentra en uso";
        public const string UsuarioInhabilitado = "El usuario no se encuentra habilitado.";

        //public static Exception ExisteCentroDeCosto { get; internal set; }
    }
}
