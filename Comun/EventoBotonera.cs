﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SGECA.Comun
{
    public class EventoBotonera : Interfaces.IEventoBotonera
    {
        public KeyEventArgs Evento { get; set; }
    }
}
