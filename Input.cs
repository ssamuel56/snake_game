using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake
{
    class Input
    {
        // Load list of avaliable keyboard inputs
        private static Hashtable keyTable = new Hashtable();

        // Perform check if button is pressed 
        public static bool KeyPressed(Keys key)
        {
            if(keyTable[key] == null)
            {
                return false;
            }

        return (bool)keyTable[key];
        }

        // Detect if keyboard button is pressed
        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
    }
}
