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
        
        public string Naslov { get; set; }
        public string ImePrezime { get; set; }

        public Posudba(int clanID, int knjigaID)
        {

            this.ClanID = clanID;
            this.KnjigaID = knjigaID;

        }

        public Posudba(int knjigaID, string naslov, string imePrezime)
        {
            this.KnjigaID = knjigaID;
            this.Naslov = naslov;
            this.ImePrezime = imePrezime;
        }

       public Posudba()
        {

        }

    }
}