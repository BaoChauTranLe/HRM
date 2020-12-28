using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRM.Models
{
	public class SetShiftViewModel
	{
		public IEnumerable<EMPLOYEE> eMPLOYEEs { get; set; }

		public IEnumerable<SHIFT> sHIFTs { get; set; }

		public SHIFTDETAIL shiftdetails { get; set; }

		public TIMEKEEPING tIMEKEEPING { get; set; }
	}
}