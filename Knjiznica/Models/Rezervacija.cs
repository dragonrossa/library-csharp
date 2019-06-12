using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knjiznica.Models
{
    public class Rezervacija
    {
        public int RezervacijaID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string NaslovKnjige { get; set; }
     

        public Rezervacija(int rezervacijaID, string ime, string prezime, string naslovKnjige)
        {
            this.RezervacijaID = rezervacijaID;
            this.Ime = ime;
            this.Prezime = prezime;
            this.NaslovKnjige = naslovKnjige;
           
        }


    }
}