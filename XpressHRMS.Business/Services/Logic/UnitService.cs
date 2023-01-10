using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.IRepository;

namespace XpressHRMS.Business.Services.ILogic
{
   public class UnitService
    {
        private readonly ILogger<UnitService> _logger;
        private readonly IUnitRepository _unitRepository;

        public UnitService(ILogger<UnitService> logger, IUnitRepository unitRepository)
        {
            _logger = logger;
            _unitRepository = unitRepository;

        }
    }
}
