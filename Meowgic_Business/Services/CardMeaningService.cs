using Meowgic.Business.Interface;
using Meowgic.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Services
{
    public class CardMeaningService : ICardMeaningService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CardMeaningService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
