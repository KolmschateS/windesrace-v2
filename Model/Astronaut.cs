using System;
namespace Model
{
    public class Astronaut : IParticipant
    {
        public string Name { get; set ; }
        public string Initial { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }

        public Astronaut(string name, int points, IEquipment equipment, TeamColors teamColor)
        {
            Name = name;
            Initial = SetInitial(name);
            Points = points;
            Equipment = equipment;
            TeamColor = teamColor;
        }
        /// <summary>
        /// Gets the first letter (initial) from the give name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>First letter of given string</returns>
        public string SetInitial(String name)
        {
            return name[0].ToString();
        }
    }
}
