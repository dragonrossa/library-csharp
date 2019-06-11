using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knjiznica.Models
{
    public class Zaposlenik
    {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
       

        public Zaposlenik(string korisnickoIme, string lozinka)
        {
            this.KorisnickoIme = korisnickoIme;
            this.Lozinka = lozinka;

        }
    }
}