﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utils.Mail;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMailController : ControllerBase
    {
        private readonly IEmailService emailService;
        public SendMailController(IEmailService service)
        {
            emailService = service;
        }

        [HttpPost]   
        public async Task<IActionResult> SendMail (string email, string userName)
        {
            try
            {
            //cria objeto para receber os dados do email a ser enviado 
            MailRequest mailRequest = new MailRequest();    
            //define endereco, assunto e corpo do email 
            mailRequest.ToEmail = email;    
            mailRequest.Subject = "Ola, esse e um email vindo da turma de dev3dm"; 
            mailRequest.Body = GetHtmlContent(userName);
            //chamar o metodo para o envio de email 
            await emailService.SendEmailAsync(mailRequest); 
            return Ok("e-mail enviado com sucesso!"); 
            }catch (Exception) 
            {
                return BadRequest("falha ao enviar o e-mail!");
            }

        }

      private string GetHtmlContent(string userName)
        {
                string Response = @"
<div style=""width:100%; background-color:rgba(96, 191, 197, 1); padding: 20px;"">
    <div style=""max-width: 600px; margin: 0 auto; background-color:#FFFFFF; border-radius: 10px; padding: 20px;"">
        <img src=""https://blobvitalhub.blob.core.windows.net/containervitalhub/logotipo.png"" alt="" Logotipo da Aplicação"" style="" display: block; margin: 0 auto; max-width: 200px;"" />
        <h1 style=""color: #333333; text-align: center;"">Bem-vindo ao VitalHub!</h1>
        <p style=""color: #666666; text-align: center;"">Olá <strong>" + userName + @"</strong>,</p>
        <p style=""color: #666666;text-align: center"">Estamos muito felizes por você ter se inscrito na plataforma VitalHub.</p>
        <p style=""color: #666666;text-align: center"">Explore todas as funcionalidades que oferecemos e encontre os melhores médicos.</p>
        <p style=""color: #666666;text-align: center"">Se tiver alguma dúvida ou precisar de assistência, nossa equipe de suporte está sempre pronta para ajudar.</p>
        <p style=""color: #666666;text-align: center"">Aproveite sua experiência conosco!</p>
        <p style=""color: #666666;text-align: center"">Atenciosamente,<br>Equipe VitalHub</p>
    </div>
</div>";

                // Retorna o conteúdo HTML do e-mail
                return Response;
            }









    }
}
