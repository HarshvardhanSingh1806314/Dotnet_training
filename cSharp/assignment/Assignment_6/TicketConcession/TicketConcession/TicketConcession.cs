namespace TicketConcession
{
    public class TicketConcession
    {
        private const float TotalFare = 500;

        public string CalculateConcession(int age)
        {
            if(age <= 5)
            {
                return "Little Champs - Free Ticket";
            }
            else if(age > 60)
            {
                float concessionFare = TotalFare * 0.30f;
                float fareAfterConcession = TotalFare - concessionFare;
                return $"Senior Citizen - Fare after 30% Concession: {fareAfterConcession}";
            }
            else
            {
                 return $"Ticket Booked - Fare: {TotalFare}";
            }
        }
    }
}
