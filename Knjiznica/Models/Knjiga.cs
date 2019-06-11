using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knjiznica.Models
{
    public class Knjiga
    {
        public int KnjigaID { get; set; }
        public string NaslovKnjige { get; set; }
        public string ImePisca { get; set; }
        public string PrezimePisca { get; set; }
        public int GodinaIzdanja { get; set; }

        public Knjiga(int knjigaId, string naslovKnjiga, string imePisca, string prezimePisca, int godinaIzdanja)
        {
            this.KnjigaID = knjigaId;
            this.NaslovKnjige = naslovKnjiga;
            this.ImePisca = imePisca;
            this.PrezimePisca = prezimePisca;
            this.GodinaIzdanja = godinaIzdanja;
        }
    }
}