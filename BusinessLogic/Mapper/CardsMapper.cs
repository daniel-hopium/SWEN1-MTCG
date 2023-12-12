using System.Collections.Generic;
using DataAccess.Daos;
using Transversal.Entities;

namespace BusinessLogic.Mapper
{
    public class CardsMapper
    {
        public static CardsDto MapToDto(CardsDao cardsDao)
        {
            if (cardsDao == null)
            {
                return null;
            }

            return new CardsDto
            {
                Id = cardsDao.Id,
                Name = cardsDao.Name,
                Damage = cardsDao.Damage
            };
        }

        public static CardsDao MapToDao(CardsDto cardsDto)
        {
            if (cardsDto == null)
            {
                return null;
            }

            return new CardsDao
            {
                Id = cardsDto.Id,
                Name = cardsDto.Name,
                Damage = cardsDto.Damage
            };
        }

        public static List<CardsDto> MapToDtoList(List<CardsDao> cardsDaoList)
        {
            if (cardsDaoList == null)
            {
                return null;
            }

            List<CardsDto> cardsDtoList = new List<CardsDto>();
            foreach (var cardsDao in cardsDaoList)
            {
                cardsDtoList.Add(MapToDto(cardsDao));
            }

            return cardsDtoList;
        }

        public static List<CardsDao> MapToDaoList(List<CardsDto> cardsDao)
        {
            if (cardsDao == null)
            {
                return null;
            }

            List<CardsDao> cardsDaoList = new List<CardsDao>();
            foreach (var cardsDto in cardsDao)
            {
                cardsDaoList.Add(MapToDao(cardsDto));
            }

            return cardsDaoList;
        }
    }
}