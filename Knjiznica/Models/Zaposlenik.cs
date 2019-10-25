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
        public string ImeZaposlenika { get; set; }
        public string PrezimeZaposlenika { get; set; }
        public string AdresaZaposlenika { get; set; }
        public int PostanskiBroj { get; set; }
       public string OdjelZaposlenika { get; set; }

        public Zaposlenik(string korisnickoIme, string lozinka)
        {
            this.KorisnickoIme = korisnickoIme;
            this.Lozinka = lozinka;

        }


        public Zaposlenik(string korisnickoIme, string lozinka, string imeZaposlenika, string prezimeZaposlenika,string adresaZaposlenika, int postanskiBroj, string odjelZaposlenika)
        {
            this.KorisnickoIme = korisnickoIme;
            this.Lozinka = lozinka;
            this.ImeZaposlenika = imeZaposlenika;
            this.PrezimeZaposlenika = prezimeZaposlenika;
            this.AdresaZaposlenika = adresaZaposlenika;
            this.PostanskiBroj = postanskiBroj;
            this.OdjelZaposlenika = odjelZaposlenika;
        }

        public Zaposlenik(string imeZaposlenika, string prezimeZaposlenika, string adresaZaposlenika, int postanskiBroj, string odjelZaposlenika)
        {
            this.ImeZaposlenika = imeZaposlenika;
            this.PrezimeZaposlenika = prezimeZaposlenika;
            this.AdresaZaposlenika = adresaZaposlenika;
            this.PostanskiBroj = postanskiBroj;
            this.OdjelZaposlenika = odjelZaposlenika;
        }

        internal static void Add(Zaposlenik zaposlenik)
        {
            throw new NotImplementedException();
        }
    }
}