using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data
{
    //defines how interact with Db, : means inherits DbContext
    /// <summary>
    /// This is the connection object for our Db
    ///</summary>
    public class ApplicationDbContext : DbContext
    {
        //constructor for init, receives config how to connect to Db
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        //add property to enable features. DbSet for Speaker Object Type called Speakers can Get/Set
            protected override void OnModelCreating(ModelBuilder modelBuilder)
                    {
            modelBuilder.Entity<Attendee>()
                .HasIndex(a => a.UserName)
                .IsUnique();

            // Ignore the computed property
            modelBuilder.Entity<Session>()
                 .Ignore(s => s.Duration);

            // Many-to-many: Conference <-> Attendee
            modelBuilder.Entity<ConferenceAttendee>()
                .HasKey(ca => new { ca.ConferenceID, ca.AttendeeID });

            // Many-to-many: Session <-> Attendee
            modelBuilder.Entity<SessionAttendee>()
                .HasKey(ca => new { ca.SessionID, ca.AttendeeID });
                
            // Many-to-many: Speaker <-> Session
            modelBuilder.Entity<SessionSpeaker>()
               .HasKey(ss => new { ss.SessionId, ss.SpeakerId });

            // Many-to-many: Session <-> Tag
            modelBuilder.Entity<SessionTag>()
                .HasKey(st => new { st.SessionID, st.TagID });
        }

        public DbSet<Conference> Conferences { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Speaker> Speakers { get; set; }

        public DbSet<Attendee> Attendees { get; set; }
    }
}