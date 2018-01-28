using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public class UberPartita : Partita
    {

        #region MessageForwarder
        //DACANCELLARE
        private void MessageHandler(UberChatCoordinamentoMessageEventArgs e)
        {
            Console.WriteLine("2) Ricevuto UberCCMEA" + e.GetType() + ": " + e);
        }
        #endregion
    }
}
