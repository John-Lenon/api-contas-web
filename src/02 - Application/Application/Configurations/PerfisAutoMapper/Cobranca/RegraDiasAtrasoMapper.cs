using Application.DTOs.Cobranca;
using AutoMapper;
using Domain.Entities.Cobranca;

namespace Application.Configurations.PerfisAutoMapper.Cobranca
{
    public class RegraDiasAtrasoMapper : Profile
    {
        public RegraDiasAtrasoMapper()
        {
            CreateMap<RegraDiaAtraso, RegraDiaAtrasoDTO>().ReverseMap();
        }
    }
}
