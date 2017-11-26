using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlemounts
{
    class AnimalRecord
    {
        public bool isSelected = false;
        public bool isException = false;
        public AnimalRecord(bool isSelected, bool isException)
        {
            this.isException = isException;
            this.isSelected = isSelected;
        }
        public override string ToString()
        {
            return this.isSelected + "," + this.isException;
        }
    }

}
