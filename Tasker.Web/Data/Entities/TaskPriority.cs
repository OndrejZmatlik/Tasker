namespace Tasker.Web.Data.Entities
{
    /// <summary>
    /// Úroveň priority školního úkolu.
    /// Hodnoty jsou uloženy jako int v databázi.
    /// </summary>
    public enum TaskPriority
    {
        /// <summary>Normální – žádné zvýraznění.</summary>
        Normal = 0,

        /// <summary>Nízká – jemné podbarvení.</summary>
        Low = 1,

        /// <summary>Střední – žlutý akcent.</summary>
        Medium = 2,

        /// <summary>Vysoká – oranžový akcent.</summary>
        High = 3,

        /// <summary>Kritická – červený pulzující efekt.</summary>
        Critical = 4
    }
}
