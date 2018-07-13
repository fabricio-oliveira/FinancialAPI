using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialApi.validate {
    public class RangeDate : RangeAttribute {

        private const string format = "dd-MM-yyyy";
       
		public RangeDate () : base (typeof (DateTime?),
                                    DateTime.Today.ToString(format),
                                    DateTime.Today.AddYears(1).ToString(format)) { }

        public override bool IsValid (object value) {
            if (value == null)
                return false;
            
            var min = DateTime.ParseExact(((string)this.Minimum), format, System.Globalization.CultureInfo.InvariantCulture);
            if (min.CompareTo (value) > 0)
                return false;
            
            var max = DateTime.ParseExact(((string)this.Maximum), format, System.Globalization.CultureInfo.InvariantCulture);
            if (max.CompareTo (value) < 0)
                return false;
            
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessageString;
        }
    }
}