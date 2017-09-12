using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StyleChooser.Models
{
    public class DbInitializer : DropCreateDatabaseAlways<DatabaseContext>
    {
        protected override void Seed(DatabaseContext db)
        {
            db.Users.Add(new User {Login = "Maksym", Password = "123"});

            db.Styles.Add(new Style {Name = "American"});
            db.Styles.Add(new Style {Name = "Asian"});
            db.Styles.Add(new Style {Name = "Classic"});
            db.Styles.Add(new Style {Name = "Country"});
            db.Styles.Add(new Style {Name = "Eclectic"});
            db.Styles.Add(new Style {Name = "Industrial"});
            db.Styles.Add(new Style {Name = "Maritime"});
            db.Styles.Add(new Style {Name = "Mediterranean"});
            db.Styles.Add(new Style {Name = "Modern"});
            db.Styles.Add(new Style {Name = "Retro"});
            db.Styles.Add(new Style {Name = "Rustic"});
            db.Styles.Add(new Style {Name = "Scandinavian"});
            db.Styles.Add(new Style {Name = "Tropical"});

            base.Seed(db);
        }
    }
}