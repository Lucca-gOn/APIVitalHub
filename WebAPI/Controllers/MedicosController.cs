﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.Domains;
using WebAPI.Interfaces;
using WebAPI.Repositories;
using WebAPI.Utils.BlobStorage;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private IMedicoRepository _medicoRepository;
        public MedicosController()
        {
            _medicoRepository = new MedicoRepository();
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_medicoRepository.ListarTodos());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BuscarPorId")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                return Ok(_medicoRepository.BuscarPorId(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task <IActionResult> Post(MedicoViewModel medicoModel)
        {
            Usuario user = new Usuario();
            user.Nome = medicoModel.Nome;
            user.Email = medicoModel.Email;
            user.TipoUsuarioId = medicoModel.IdTipoUsuario;
            user.Senha = medicoModel.Senha;

            //Insere a foto de perfil
            var containerName = "";
            var connectionString = "";
            user.Foto = await AzureBlobStorageHelper.UploadImageBlobAsync(medicoModel.Arquivo!, connectionString, containerName);

            user.Medico = new Medico();
            user.Medico.Crm = medicoModel.Crm;
            user.Medico.EspecialidadeId = medicoModel.EspecialidadeId;


            user.Medico.Endereco = new Endereco();
            user.Medico.Endereco.Logradouro = medicoModel.Logradouro;
            user.Medico.Endereco.Numero = medicoModel.Numero;
            user.Medico.Endereco.Cep = medicoModel.Cep;

            _medicoRepository.Cadastrar(user);

            return Ok(user);
        }

        [HttpGet("BuscarPorIdClinica")]
        public IActionResult GetByIdClinica(Guid id)
        {
            try
            {
                return Ok(_medicoRepository.ListarPorClinica(id)); ;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BuscarPorData")]
        public IActionResult GetByDate(DateTime data, Guid id)
        {
            try
            {
                return Ok(_medicoRepository.BuscarPorData(data, id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateProfile(MedicoViewModel medico)
        {
            try
            {
                Guid idUsuario = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value);

                return Ok(_medicoRepository.AtualizarPerfil(idUsuario, medico));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
