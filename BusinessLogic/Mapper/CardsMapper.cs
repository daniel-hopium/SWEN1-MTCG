using DataAccess.Daos;
using Transversal.Entities;

namespace BusinessLogic.Mapper
{
    public class CardsMapper
    {
        public static CardDto MapToDto(CardDao cardDao)
        {
            if (cardDao == null)
            {
                return null;
            }

            return new CardDto
            {
                Id = cardDao.Id,
                Name = cardDao.Name,
                Damage = cardDao.Damage,
                ElementType = cardDao.ElementType,
                CardType = cardDao.CardType
            };
        }

        public static CardDao MapToDao(CardDto cardDto)
        {
            if (cardDto == null)
            {
                return null;
            }

            return new CardDao
            {
                Id = cardDto.Id,
                Name = cardDto.Name,
                Damage = cardDto.Damage,
                ElementType = cardDto.ElementType,
                CardType = cardDto.CardType
                    
            };
        }

        public static List<CardDto> MapToDtoList(List<CardDao> cardsDaoList)
        {
            if (cardsDaoList == null)
            {
                return null;
            }

            List<CardDto> cardsDtoList = new List<CardDto>();
            foreach (var cardsDao in cardsDaoList)
            {
                cardsDtoList.Add(MapToDto(cardsDao));
            }

            return cardsDtoList;
        }

        public static List<CardDao> MapToDaoList(List<CardDto> cardsDao)
        {
            if (cardsDao == null)
            {
                return null;
            }

            List<CardDao> cardsDaoList = new List<CardDao>();
            foreach (var cardsDto in cardsDao)
            {
                cardsDaoList.Add(MapToDao(cardsDto));
            }

            return cardsDaoList;
        }
    }
}