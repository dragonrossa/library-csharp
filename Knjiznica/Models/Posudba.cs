using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knjiznica.Models
{
    public class Posudba
    {
        public int PosudbaID { get; set; }
        public int ClanID { get; set; }
        public int KnjigaID { get; set; }
        public DateTime DatumPosudbe { get; set; }
        public DateTime DatumVracanja { get; set; }

        public Posudba(int clanID, int knjigaID)
        {
           
            this.ClanID = clanID;
            this.KnjigaID = knjigaID;
          
        }
    }
}