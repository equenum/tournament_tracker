using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    /// <summary>
    /// Represents an interface for different output data connection types.
    /// </summary>
    public interface IDataConnection
    {
        PrizeModel CreatePrize(PrizeModel model);
    }
}
