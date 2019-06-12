using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using Knjiznica.Models;
using System.Data;

namespace Knjiznica.Controllers
{
    public class PregledController : Controller
    {

        private List<Knjiga> ListaKnjiga = new List<Knjiga>();


        private List<Rezervacija> ListaRezervacija = new List<Rezervacija>();


        private string ConnStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: Pregled
        public ActionResult Index()
        {

            string freeBooksQuery = "SELECT * FROM dbo.free_books";

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(freeBooksQuery, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Debug.WriteLine("\t{0}\t{1}\t{2}t{3}\t{4}",
                            reader[0], reader[1], reader[2], reader[3], reader[4]);

                        ListaKnjiga.Add(new Knjiga(Int32.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(),
                            reader[3].ToString(), Int32.Parse(reader[4].ToString())));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            //ViewBag.TotalStudents = ListaKnjiga.Count;
            return View(ListaKnjiga);
        }

        public ActionResult Rezervacija(int id)
        {
            Debug.WriteLine("NUTRA SAM " + id);
            return View();  
        }

        [HttpPost]
        public ActionResult RezervacijaStatus(FormCollection form, int test)
        {


            int knjigaID = test;
           // Debug.WriteLine(knjigaID);

            // TODO: Back end validation

            Clan clan = new Clan(form["Ime"], form["Prezime"], form["Adresa"],
                form["Email"], form["Telefon"]);

           

            Debug.WriteLine("IME: " + clan.Ime);
            Debug.WriteLine("PREZIME: " + clan.Prezime);
            Debug.WriteLine("ADRESA: " + clan.AdresaStanovanja);
            Debug.WriteLine("EMAIL: " + clan.Email);
            Debug.WriteLine("TELEFON: " + clan.Telefon);

            

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                try
                {

                 connection.Open();

 

                SqlCommand command = new SqlCommand("INSERT INTO Clanovi(Ime, Prezime, AdresaStanovanja, Email, Telefon) output INSERTED.ClanID VALUES (@Ime, @Prezime, @Adresa, @Email, @Telefon)", connection);

                  
                    command.Parameters.Add("@Ime", SqlDbType.VarChar, 50).Value = clan.Ime;
                    command.Parameters.Add("@Prezime", SqlDbType.VarChar, 50).Value = clan.Prezime;
                    command.Parameters.Add("@Adresa", SqlDbType.VarChar, 100).Value = clan.AdresaStanovanja;
                    command.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = clan.Email;
                    command.Parameters.Add("@Telefon", SqlDbType.VarChar, 50).Value = clan.Telefon;


                    int clanID = (int)command.ExecuteScalar();
           
                    Debug.WriteLine(clanID);


                    Debug.WriteLine("Uspješno upisan novi član!");

                    connection.Close();


                    /****************************POSUDBA******************************/

                    Posudba posudba = new Posudba(clanID, knjigaID);

                    Debug.WriteLine(posudba.ClanID);
                    Debug.WriteLine(posudba.KnjigaID);

                    connection.Open();

                    SqlCommand command2 = new SqlCommand("INSERT INTO Posudba(ClanID, KnjigaID) VALUES (@ClanID, @KnjigaID)", connection);

                    command2.Parameters.Add("ClanID", SqlDbType.Int, 16).Value = posudba.ClanID;
                    command2.Parameters.Add("KnjigaID", SqlDbType.Int, 16).Value = posudba.KnjigaID;

                    command2.ExecuteNonQuery();

                    Debug.WriteLine("Uspješno upisana nova posudba!");

                    connection.Close();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }



            }


            return View();
        }


        //REGISTRATIONS

       
        public ActionResult Registracija()
        {
   
            return View();
        }


        [HttpPost]
        //[Authorize(Users = "Rosana")]
        public ActionResult RegistracijaUspjesna(FormCollection form)
        {
            Zaposlenik zaposlenik = new Zaposlenik(form["KorisnickoIme"], form["Lozinka"]);

            Debug.WriteLine("Korisnicko ime: " + zaposlenik.KorisnickoIme);
            Debug.WriteLine("Lozinka: " + zaposlenik.Lozinka);

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                try
                {

                    connection.Open();



                    SqlCommand command = new SqlCommand("INSERT INTO Zaposlenici VALUES (@KorisnickoIme, @Lozinka)", connection);


                    command.Parameters.Add("@KorisnickoIme", SqlDbType.VarChar, 50).Value = zaposlenik.KorisnickoIme;
                    command.Parameters.Add("@Lozinka", SqlDbType.VarChar, 50).Value = zaposlenik.Lozinka;

                    command.ExecuteNonQuery();




                    Debug.WriteLine("Uspješno upisan novi zaposlenik!");

                    connection.Close();




                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }


                return View();
        }


        public ActionResult Prijava()
        {
            return View();
        }

        //[Authorize(Users = "Ivica")]

        [HttpPost]
        public ActionResult PrijavaUspjesna(FormCollection form)
        {
            string korisnickoIme = form["KorisnickoIme"];
            string lozinka = form["Lozinka"];

            Debug.WriteLine("Korisnicko ime: " + korisnickoIme);
            Debug.WriteLine("Lozinka: " + lozinka);


            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                try
                {

            
                    SqlCommand command = new SqlCommand
                        ("IF EXISTS (SELECT * FROM Zaposlenici WHERE KorisnickoIme = @KorisnickoIme AND Lozinka=@Lozinka) SELECT 1 ELSE SELECT 2", connection);

                    command.Parameters.Add("@KorisnickoIme", SqlDbType.VarChar, 50).Value = korisnickoIme;
                    command.Parameters.Add("@Lozinka", SqlDbType.VarChar, 50).Value = lozinka;



                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Debug.WriteLine("{0}",reader[0]);

                            if (reader[0].ToString() == "1") 
                            {
                                return View("PrijavaUspjesna");
                            }
                            else
                            {
                                return View("PrijavaNeuspjesna");
                            }
                           
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }




                    connection.Close();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

           

            return View();


            
        }


        public ActionResult PrijavaNeuspjesna()
        {
          
            return View();
        }

       
       
        public ActionResult PregledRezervacija()
        {

 
            return View();


        }

        public ActionResult Rezervacijaa()
        {
            string rezervacijaQuery = "SELECT * from rezervacijas";

            Debug.WriteLine("Hej");

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(rezervacijaQuery, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Debug.Write("Zašto se ne vidi");
                        Debug.WriteLine("Šta je ovo...{0},{1},{2},{3}", reader[0], reader[1], reader[2], reader[3]);

                        ListaRezervacija.Add(new Rezervacija
                            (Int32.Parse(reader[0].ToString()), reader[1].ToString(),
                            reader[2].ToString(), reader[3].ToString()));



                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return View(ListaRezervacija);
      
        }


       

  
        public ActionResult Knjige()
        {

            string freeBooksQuery = "SELECT * FROM Knjige";

            using (SqlConnection connection = new SqlConnection(ConnStr))
            {
                SqlCommand command = new SqlCommand(freeBooksQuery, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Debug.WriteLine("\t{0}\t{1}\t{2}t{3}\t{4}",
                            reader[0], reader[1], reader[2], reader[3], reader[4]);

                        ListaKnjiga.Add(new Knjiga(Int32.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(),
                            reader[3].ToString(), Int32.Parse(reader[4].ToString())));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
           
            return View(ListaKnjiga);
 

                
        }

        public ActionResult Odjava()
        {
            return View();
        }
    }
}