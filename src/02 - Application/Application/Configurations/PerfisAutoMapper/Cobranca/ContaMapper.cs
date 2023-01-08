using Application.DTOs.Cobranca;
using AutoMapper;
using Domain.Entities.Cobranca;

namespace Application.Configurations.AutoMapper.Cobranca
{
    public class ContaMapper : Profile
    {
        public ContaMapper()
        {
            CreateMap<ContaDTO, Conta>().ReverseMap();
            CreateMap<Conta, ContaGetDTO>();
        }
    }
}
