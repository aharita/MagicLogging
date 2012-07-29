using System;

namespace MagicLogging.Demo
{
    [MagicLogging("Id", "Name")]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    [MagicLogging]
    public class Achievement
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var user = new User
            {
                Id = 412,
                Name = "Alfonso Harita",
                Password = "secret"
            };

            var achievement = new Achievement
            {
                Id = 24917,
                User = user,
                Description = "Learn AOP"
            };

            var result = MagicLoggingAttribute.MagicLog(achievement);
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
