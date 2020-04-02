namespace iTrellisProject.Models
{
    /**
     * Domain model for holidays i.e. days where we don't ship (no view model)
     */
    public class Holiday
    {
        public string name { get; set; }
        public int month { get; set; }
        public int day { get; set; }
    }
}