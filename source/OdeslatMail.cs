using System.Net;
using System.Net.Mail;

namespace BudizMail
{
    class OdeslatMail
    {
        public string
            smtpAdresa       = "",    // adresa zadaného SMTP server
            smtpPort         = "",    // číslo TCP portu pro připojení k zadanému SMTP
            uzivatel         = "",    // přihlašovací jméno uživatele pro SMTP (nepovinné)
            heslo            = "",    // heslo uživatele pro SMTP (nepovinné)

            jmenoOdesilatele = "",    // jméno odesílatele (nepovinné)
            emailOdesilatele = "",    // e-mail odesílatele zprávy
            emailPrijemce    = "",    // e-mail příjemce zprávy

            predmet          = "",    // předmět zprávy (nepovinné)
            zprava           = "",    // text zprávy (nepovinné)


            chyba            = "";    // při výjimce "catch" (return false) obsahuje detaily o výjimce

        public bool
            ssl;                      // nastavuje zapnutí či vypnutí SSL u u SmtpClient.EnableSsl

        private const int
            casovyLimitSmtp = 20000;  // časový limit pro SMTP


        public bool Posli()
        {

            try
            {
                SmtpClient smtp = new SmtpClient(smtpAdresa, int.Parse(smtpPort));  // objekt pro připojení k SMTP
                smtp.Credentials = new NetworkCredential(uzivatel, heslo); // nastavení přihlašovacích údajů pro SMTP
                smtp.EnableSsl = ssl; // zapnutí či vypnutí SSL dle hodnoty v proměnné ssl (true/false)
                smtp.Timeout = casovyLimitSmtp; // časový limit pro SMTP
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                // použít či nepoužít přihlašovací údaje pro SMTP
                if ((uzivatel != "") || (heslo != ""))
                {
                    smtp.UseDefaultCredentials = false;
                }
                else
                {
                    smtp.UseDefaultCredentials = true;
                }

                MailAddress odesilatel = new MailAddress(emailOdesilatele, jmenoOdesilatele); // první parametr pro MailMessage
                MailAddress prijemce = new MailAddress(emailPrijemce, null); // druhý parametr pro MailMessage

                MailMessage mail = new MailMessage(odesilatel, prijemce);
                mail.Subject = predmet;
                mail.Body = zprava;

                smtp.Send(mail); // odeslání mailu

                return true;
            }
            catch (Exception ex)
            {
                chyba = ex.Message;
                return false;
            }
        }
    }
}
