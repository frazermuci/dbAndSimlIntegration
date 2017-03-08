using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class DataAccessDB : DataAccess
    {
        //private ActivAIDDB db;

        public DataAccessDB()
        {
            ;//db = new ActivAIDDB();
        }

        public Dictionary<int, List<string>> query(Query query)
        {
            //return db.getAllElements(query.attributeList.First().value);
            Dictionary<int, List<string>> dList = new Dictionary<int, List<string>>();
            dList[0] = new List<string>(new string[] { "Use the Login Manager to add a new user account for the application.&nbsp;" });
            dList[1] = new List<string>(new string[]{
                "Click the new user button in the upper left corner or right-click on an empty line in the user list and select Add New User."
                ,"The fields on the left side of the form will now be enabled.&nbsp; Enter the User ID for the new account.&nbsp; The User ID must be unique."
                ,"Enter a description (optional).",
                "Enter the password in the Password field.&nbsp; Check the Show Password as Text During Edit to show the real password characters instead of asterisks (*).",
                "Re-enter the same password in the Confirm Password field.",
                "Select the group the user is to be part of.",
                "Click the Create User button to save the new user.&nbsp; If successful, the new user account will appear in the user list."});
            dList[2] = new List<string>(new string[] { "&nbsp;" });
            dList[3] = new List<string>(new string[] { "&nbsp;&nbsp;&nbsp; Login Manager | a user| Deleting a user" });
            dList[4] = new List<string>(new string[] { "&nbsp;" });
            dList[5] = new List<string>(new string[] { "Back to Top" });
            dList[6] = new List<string>(new string[] { "Astronics Test Systems Last updated on 3/1/07 by L. Anhalt" });
            return dList;
        }
    }
}
