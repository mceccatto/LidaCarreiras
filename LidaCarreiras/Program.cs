using LidaCarreiras.Classes;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace LidaCarreiras
{
    public class Program
    {
        static string Saudacao()
        {
            return "WebAPI Lida Carreiras";
        }

        record RegistrarCandidato(string nome, string nomeSocial, string genero, string dataNascimento, string telefone, string email, string senha);
        record LoginCandidatoEmpresa(string email, string senha);
        record EditarCandidato(long id, string nome, string nomeSocial, string genero, string dataNascimento, string telefone, string sobre, string senha);
        record RegistrarEditarCandidatoEmpresaEndereco(long id, string logradouro, string numero, string complemento, string cep, string bairro, string cidade, string estado);
        record RegistrarEditarCandidatoConhecimento(long id, JsonArray conhecimento);
        record RegistrarCandidatoFormacao(long id, string instituicao, string curso, string dataInicio, string dataConclusao);
        record EditarCandidatoFormacao(long id, string instituicao, string curso, string dataInicio, string dataConclusao, string dataCadastro);
        record RegistrarCandidatoExperiencia(long id, string empresa, string funcao, string dataContratacao, string dataDesligamento, string descricao);
        record EditarCandidatoExperiencia(long id, string empresa, string funcao, string dataContratacao, string dataDesligamento, string descricao, string dataCadastro);
        record RegistrarCandidatoCertificado(long id, string instituicao, string titulo, string anexo, string url);
        record EditarCandidatoCertificado(long id, string instituicao, string titulo, string anexo, string url, string dataCadastro);
        record RegistrarEmpresa(string cnpj, string rasaoSocial, string nomeFantasia, string telefone, string ramal, string email, string senha);
        record EditarEmpresa(long id, string rasaoSocial, string nomeFantasia, string telefone, string ramal, string descricao, string logo, string senha);
        record RegistrarEmpresaVaga(long id, string titulo, string descricao, long modalidade, long modelo, long area, string dataLimite);
        record EditarEmpresaVaga(long id, string titulo, string descricao, long modalidade, long modelo, long area, string dataLimite, bool status);

        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();

            app.Urls.Add("http://*:8888");

            app.MapGet("/", Saudacao);

            var registrarCandidato = (RegistrarCandidato dados) => {
                if (dados.nome == null || dados.genero == null || dados.dataNascimento == null || dados.telefone == null || dados.email == null || dados.senha == null)
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.nome == null)
                    {
                        camposInvalidos += "Nome, ";
                    }
                    if (dados.genero == null)
                    {
                        camposInvalidos += "Gênero, ";
                    }
                    if (dados.dataNascimento == null)
                    {
                        camposInvalidos += "Data de Nascimento, ";
                    }
                    if (dados.telefone == null)
                    {
                        camposInvalidos += "Telefone, ";
                    }
                    if (dados.email == null)
                    {
                        camposInvalidos += "E-mail, ";
                    }
                    if (dados.senha == null)
                    {
                        camposInvalidos += "Senha, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }
                Candidato candidato = new Candidato(dados.nome, dados.nomeSocial, dados.genero, dados.dataNascimento, Regex.Replace(dados.telefone, @"[^0-9]+", ""), dados.email, dados.senha, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), true);
                return Candidato.RegistrarCandidato(candidato);
            };
            app.MapPost("/candidato/registrar", registrarCandidato);

            var loginCandidato = (LoginCandidatoEmpresa dados) => {
                if (dados.email == null || dados.senha == null)
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.email == null)
                    {
                        camposInvalidos += "E-mail, ";
                    }
                    if (dados.senha == null)
                    {
                        camposInvalidos += "Senha, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }

                Candidato candidato = new Candidato(dados.email, dados.senha);
                return Candidato.LoginCandidato(candidato);
            };
            app.MapPost("/candidato/login", loginCandidato);

            var recuperarSenhaCandidato = (string email) => {
                if (email == null)
                {
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", "É obrigatório informar um E-mail!");
                    return retornoVerificacao;
                }
                Candidato candidato = new Candidato(email);
                return Candidato.RecuperarSenhaCandidato(candidato);
            };
            app.MapGet("/candidato/recuperar-senha/{email}", recuperarSenhaCandidato);

            var editarCandidato = (EditarCandidato dados) => {
                if (dados.nome == null || dados.genero == null || dados.dataNascimento == null || dados.telefone == null)
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.nome == null)
                    {
                        camposInvalidos += "Nome, ";
                    }
                    if (dados.genero == null)
                    {
                        camposInvalidos += "Gênero, ";
                    }
                    if (dados.dataNascimento == null)
                    {
                        camposInvalidos += "Data de Nascimento, ";
                    }
                    if (dados.telefone == null)
                    {
                        camposInvalidos += "Telefone, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }
                Candidato candidato = new Candidato(dados.id, dados.nome, dados.nomeSocial, dados.genero, dados.dataNascimento, Regex.Replace(dados.telefone, @"[^0-9]+", ""), dados.sobre, dados.senha);
                return Candidato.EditarCandidato(candidato);
            };
            app.MapPost("/candidato/editar", editarCandidato);

            var consultarCandidato = (long id) => {
                Candidato candidato = new Candidato(id);
                return Candidato.ConsultarCandidato(candidato);
            };
            app.MapGet("/candidato/consultar/{id}", consultarCandidato);

            var registrarEditarCandidatoEndereco = (RegistrarEditarCandidatoEmpresaEndereco dados) => {
                if (dados.logradouro == null || dados.numero == null || dados.complemento == null || dados.cep == null || dados.bairro == null || dados.cidade == null || dados.estado == null)
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.logradouro == null)
                    {
                        camposInvalidos += "Logradouro, ";
                    }
                    if (dados.numero == null)
                    {
                        camposInvalidos += "Número, ";
                    }
                    if (dados.complemento == null)
                    {
                        camposInvalidos += "Complemento, ";
                    }
                    if (dados.cep == null)
                    {
                        camposInvalidos += "Cep, ";
                    }
                    if (dados.bairro == null)
                    {
                        camposInvalidos += "Bairro, ";
                    }
                    if (dados.cidade == null)
                    {
                        camposInvalidos += "Cidade, ";
                    }
                    if (dados.estado == null)
                    {
                        camposInvalidos += "Estado, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }
                CandidatoEndereco endereco = new CandidatoEndereco(dados.id, dados.logradouro, dados.numero, dados.complemento, Regex.Replace(dados.cep, @"[^0-9]+", ""), dados.bairro, dados.cidade, dados.estado, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
                if (!CandidatoEndereco.ConsultarCadastroEndereco(dados.id))
                {
                    return CandidatoEndereco.RegistrarEndereco(endereco);
                } else
                {
                    return CandidatoEndereco.EditarEndereco(endereco);
                }
            };
            app.MapPost("/candidato/endereco/registrar", registrarEditarCandidatoEndereco);

            var consultarCandidatoEndereco = (long id) => {
                CandidatoEndereco endereco = new CandidatoEndereco(id);
                return CandidatoEndereco.ConsultarEndereco(endereco);
            };
            app.MapGet("/candidato/endereco/consultar/{id}", consultarCandidatoEndereco);

            var excluirCandidatoEndereco = (long id) => {
                CandidatoEndereco endereco = new CandidatoEndereco(id);
                return CandidatoEndereco.ExcluirEndereco(endereco);
            };
            app.MapGet("/candidato/endereco/excluir/{id}", excluirCandidatoEndereco);

            var registrarEditarCandidatoConhecimento = (RegistrarEditarCandidatoConhecimento dados) => {
                /*if (dados.conhecimento.Count == 0)
                {
                    var camposInvalidos = "Campo inválido: ";
                    if (dados.conhecimento == null)
                    {
                        camposInvalidos += "Conhecimento, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }*/
                CandidatoConhecimento conhecimento = new CandidatoConhecimento(dados.id, dados.conhecimento);
                if (!CandidatoConhecimento.ConsultarCadastroConhecimento(conhecimento))
                {
                    return CandidatoConhecimento.RegistrarConhecimento(conhecimento);
                }
                else
                {
                    return CandidatoConhecimento.EditarConhecimento(conhecimento);
                }
            };
            app.MapPost("/candidato/conhecimento/registrar", registrarEditarCandidatoConhecimento);

            var consultarCandidatoConhecimento = (long id) => {
                CandidatoConhecimento conhecimento = new CandidatoConhecimento(id);
                return CandidatoConhecimento.ConsultarConhecimento(conhecimento);
            };
            app.MapGet("/candidato/conhecimento/consultar/{id}", consultarCandidatoConhecimento);

            var excluirCandidatoConhecimento = (long id) => {
                CandidatoConhecimento conhecimento = new CandidatoConhecimento(id);
                return CandidatoConhecimento.ExcluirConhecimento(conhecimento);
            };
            app.MapGet("/candidato/conhecimento/excluir/{id}", excluirCandidatoConhecimento);

            var registrarCandidatoFormacao = (RegistrarCandidatoFormacao dados) => {
                if (dados.instituicao == null || dados.curso == null || dados.dataInicio == null)
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.instituicao == null)
                    {
                        camposInvalidos += "Instituição, ";
                    }
                    if (dados.curso == null)
                    {
                        camposInvalidos += "Curso, ";
                    }
                    if (dados.dataInicio == null)
                    {
                        camposInvalidos += "Data de Início, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }
                CandidatoFormacao formacao = new CandidatoFormacao(dados.id, dados.instituicao, dados.curso, dados.dataInicio, dados.dataConclusao, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
                return CandidatoFormacao.RegistrarFormacao(formacao);
            };
            app.MapPost("/candidato/formacao/registrar", registrarCandidatoFormacao);

            var consultarCandidatoFormacao = (long id) => {
                CandidatoFormacao formacao = new CandidatoFormacao(id);
                return CandidatoFormacao.ConsultarFormacao(formacao);
            };
            app.MapGet("/candidato/formacao/consultar/{id}", consultarCandidatoFormacao);

            var excluirCandidatoFormacao = (long id, string data) => {
                CandidatoFormacao formacao = new CandidatoFormacao(id,data);
                return CandidatoFormacao.ExcluirFormacao(formacao);
            };
            app.MapGet("/candidato/formacao/excluir/{id}/{data}", excluirCandidatoFormacao);

            var editarCandidatoFormacao = (EditarCandidatoFormacao dados) => {
                if (dados.instituicao == null || dados.curso == null || dados.dataInicio == null)
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.instituicao == null)
                    {
                        camposInvalidos += "Instituição, ";
                    }
                    if (dados.curso == null)
                    {
                        camposInvalidos += "Curso, ";
                    }
                    if (dados.dataInicio == null)
                    {
                        camposInvalidos += "Data de Início, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }
                CandidatoFormacao formacao = new CandidatoFormacao(dados.id, dados.instituicao, dados.curso, dados.dataInicio, dados.dataConclusao, dados.dataCadastro);
                return CandidatoFormacao.EditarFormacao(formacao);
            };
            app.MapPost("/candidato/formacao/editar", editarCandidatoFormacao);

            var registrarCandidatoExperiencia = (RegistrarCandidatoExperiencia dados) => {
                if (dados.empresa == null || dados.funcao == null || dados.descricao == null || dados.dataContratacao == null)
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.empresa == null)
                    {
                        camposInvalidos += "Empresa, ";
                    }
                    if (dados.funcao == null)
                    {
                        camposInvalidos += "Função, ";
                    }
                    if (dados.dataContratacao == null)
                    {
                        camposInvalidos += "Contratação, ";
                    }
                    if (dados.descricao == null)
                    {
                        camposInvalidos += "Descrição, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }
                CandidatoExperiencia experiencia = new CandidatoExperiencia(dados.id, dados.empresa, dados.funcao, dados.dataContratacao, dados.dataDesligamento, dados.descricao, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
                return CandidatoExperiencia.RegistrarExperiencia(experiencia);
            };
            app.MapPost("/candidato/experiencia/registrar", registrarCandidatoExperiencia);

            var consultarCandidatoExperiencia = (long id, string data) => {
                CandidatoExperiencia experiencia = new CandidatoExperiencia(id,data);
                return CandidatoExperiencia.ConsultarExperiencia(experiencia);
            };
            app.MapGet("/candidato/experiencia/consultar/{id}/{data}", consultarCandidatoExperiencia);

            var editarExperiencia = (EditarCandidatoExperiencia dados) => {
                if (dados.empresa == null || dados.funcao == null || dados.descricao == null || dados.dataContratacao == null)
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.empresa == null)
                    {
                        camposInvalidos += "Empresa, ";
                    }
                    if (dados.funcao == null)
                    {
                        camposInvalidos += "Função, ";
                    }
                    if (dados.dataContratacao == null)
                    {
                        camposInvalidos += "Contratação, ";
                    }
                    if (dados.descricao == null)
                    {
                        camposInvalidos += "Descrição, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }
                CandidatoExperiencia experiencia = new CandidatoExperiencia(dados.id, dados.empresa, dados.funcao, dados.dataContratacao, dados.dataDesligamento, dados.descricao, dados.dataCadastro);
                return CandidatoExperiencia.EditarExperiencia(experiencia);
            };
            app.MapPost("/candidato/experiencia/editar", editarExperiencia);

            var excluirCandidatoExperiencia = (long id, string data) => {
                CandidatoExperiencia experiencia = new CandidatoExperiencia(id, data);
                return CandidatoExperiencia.ExcluirExperiencia(experiencia);
            };
            app.MapGet("/candidato/experiencia/excluir/{id}/{data}", excluirCandidatoExperiencia);

            var registrarCandidatoCertificado = (RegistrarCandidatoCertificado dados) => {
                if (dados.instituicao == null || dados.titulo == null || (dados.anexo == null && dados.url == null))
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.instituicao == null)
                    {
                        camposInvalidos += "Instituição, ";
                    }
                    if (dados.titulo == null)
                    {
                        camposInvalidos += "Titulo, ";
                    }
                    if (dados.anexo == null && dados.url == null)
                    {
                        camposInvalidos += "Você deve inserir um anexo ou informar uma url, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }
                CandidatoCertificado certificado = new CandidatoCertificado(dados.id, dados.instituicao, dados.titulo, dados.anexo, dados.url, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
                return CandidatoCertificado.RegistrarCertificado(certificado);
            };
            app.MapPost("/candidato/certificado/registrar", registrarCandidatoCertificado);

            var consultarCandidatoCertificado = (long id) => {
                CandidatoCertificado certificado = new CandidatoCertificado(id);
                return CandidatoCertificado.ConsultarCertificado(certificado);
            };
            app.MapGet("/candidato/certificado/consultar/{id}", consultarCandidatoCertificado);

            var excluirCandidatoCertificado = (long id, string data) => {
                CandidatoCertificado certificado = new CandidatoCertificado(id, data);
                return CandidatoCertificado.ExcluirCertificado(certificado);
            };
            app.MapGet("/candidato/certificado/excluir/{id}/{data}", excluirCandidatoCertificado);

            var registrarEmpresa = (RegistrarEmpresa dados) => {
                if (dados.cnpj == null || dados.rasaoSocial == null || dados.nomeFantasia == null || dados.telefone == null || dados.email == null || dados.senha == null)
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.cnpj == null)
                    {
                        camposInvalidos += "CNPJ, ";
                    }
                    if (dados.rasaoSocial == null)
                    {
                        camposInvalidos += "Rasão Social, ";
                    }
                    if (dados.nomeFantasia == null)
                    {
                        camposInvalidos += "Nome Fantasia, ";
                    }
                    if (dados.telefone == null)
                    {
                        camposInvalidos += "Telefone, ";
                    }
                    if (dados.email == null)
                    {
                        camposInvalidos += "E-mail, ";
                    }
                    if (dados.senha == null)
                    {
                        camposInvalidos += "Senha, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }
                Empresa empresa = new Empresa(Regex.Replace(dados.cnpj, @"[^0-9]+", ""), dados.rasaoSocial, dados.nomeFantasia, Regex.Replace(dados.telefone, @"[^0-9]+", ""), dados.ramal, dados.email, dados.senha, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), true);
                return Empresa.RegistrarEmpresa(empresa);
            };
            app.MapPost("/empresa/registrar", registrarEmpresa);

            var loginEmpresa = (LoginCandidatoEmpresa dados) => {
                if (dados.email == null || dados.senha == null)
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.email == null)
                    {
                        camposInvalidos += "E-mail, ";
                    }
                    if (dados.senha == null)
                    {
                        camposInvalidos += "Senha, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }

                Empresa empresa = new Empresa(dados.email, dados.senha);
                return Empresa.LoginEmpresa(empresa);
            };
            app.MapPost("/empresa/login", loginEmpresa);

            var editarEmpresa = (EditarEmpresa dados) => {
                if (dados.rasaoSocial == null || dados.nomeFantasia == null || dados.telefone == null)
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.rasaoSocial == null)
                    {
                        camposInvalidos += "Rasão Social, ";
                    }
                    if (dados.nomeFantasia == null)
                    {
                        camposInvalidos += "Nome Fantasia, ";
                    }
                    if (dados.telefone == null)
                    {
                        camposInvalidos += "Telefone, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }
                Empresa empresa = new Empresa(dados.id, dados.rasaoSocial, dados.nomeFantasia, Regex.Replace(dados.telefone, @"[^0-9]+", ""), dados.ramal, dados.descricao, dados.logo, dados.senha);
                return Empresa.EditarEmpresa(empresa);
            };
            app.MapPost("/empresa/editar", editarEmpresa);

            var consultarEmpresa = (long id) => {
                Empresa empresa = new Empresa(id);
                return Empresa.ConsultarEmpresa(empresa);
            };
            app.MapGet("/empresa/consultar/{id}", consultarEmpresa);

            var registrarEditarEmpresaEndereco = (RegistrarEditarCandidatoEmpresaEndereco dados) => {
                if (dados.logradouro == null || dados.numero == null || dados.cep == null || dados.bairro == null || dados.cidade == null || dados.estado == null)
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.logradouro == null)
                    {
                        camposInvalidos += "Logradouro, ";
                    }
                    if (dados.numero == null)
                    {
                        camposInvalidos += "Número, ";
                    }
                    if (dados.cep == null)
                    {
                        camposInvalidos += "Cep, ";
                    }
                    if (dados.bairro == null)
                    {
                        camposInvalidos += "Bairro, ";
                    }
                    if (dados.cidade == null)
                    {
                        camposInvalidos += "Cidade, ";
                    }
                    if (dados.estado == null)
                    {
                        camposInvalidos += "Estado, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }
                EmpresaEndereco endereco = new EmpresaEndereco(dados.id, dados.logradouro, dados.numero, dados.complemento, Regex.Replace(dados.cep, @"[^0-9]+", ""), dados.bairro, dados.cidade, dados.estado, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
                if (!EmpresaEndereco.ConsultarCadastroEndereco(dados.id))
                {
                    return EmpresaEndereco.RegistrarEndereco(endereco);
                }
                else
                {
                    return EmpresaEndereco.EditarEndereco(endereco);
                }
            };
            app.MapPost("/empresa/endereco/registrar", registrarEditarEmpresaEndereco);

            var consultarEmpresaEndereco = (long id) => {
                EmpresaEndereco endereco = new EmpresaEndereco(id);
                return EmpresaEndereco.ConsultarEndereco(endereco);
            };
            app.MapGet("/empresa/endereco/consultar/{id}", consultarEmpresaEndereco);

            var excluirEmpresaEndereco = (long id) => {
                EmpresaEndereco endereco = new EmpresaEndereco(id);
                return EmpresaEndereco.ExcluirEndereco(endereco);
            };
            app.MapGet("/empresa/endereco/excluir/{id}", excluirEmpresaEndereco);

            var registrarEmpresaVaga = (RegistrarEmpresaVaga dados) => {
                if (dados.titulo == null || dados.descricao == null || dados.modalidade == 0 || dados.modelo == 0 || dados.area == 0)
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.titulo == null)
                    {
                        camposInvalidos += "Título, ";
                    }
                    if (dados.descricao == null)
                    {
                        camposInvalidos += "Descrição, ";
                    }
                    if (dados.modalidade == 0)
                    {
                        camposInvalidos += "Modalidade, ";
                    }
                    if (dados.modelo == 0)
                    {
                        camposInvalidos += "Modelo, ";
                    }
                    if (dados.area == 0)
                    {
                        camposInvalidos += "Área, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }
                EmpresaVaga vaga = new EmpresaVaga(dados.id, dados.titulo, dados.descricao, dados.modalidade, dados.modelo, dados.area, dados.dataLimite, true, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
                return EmpresaVaga.RegistrarVaga(vaga);
            };
            app.MapPost("/empresa/vaga/registrar", registrarEmpresaVaga);

            var consultarEmpresaVaga = (long id) => {
                EmpresaVaga vaga = new EmpresaVaga(id);
                return EmpresaVaga.ConsultarVagas(vaga);
            };
            app.MapGet("/empresa/vagas/consultar/{id}", consultarEmpresaVaga);

            var editarEmpresaVaga = (EditarEmpresaVaga dados) => {
                if (dados.titulo == null || dados.descricao == null || dados.modalidade == 0 || dados.modelo == 0 || dados.area == 0)
                {
                    var camposInvalidos = "Campos inválidos: ";
                    if (dados.titulo == null)
                    {
                        camposInvalidos += "Título, ";
                    }
                    if (dados.descricao == null)
                    {
                        camposInvalidos += "Descrição, ";
                    }
                    if (dados.modalidade == 0)
                    {
                        camposInvalidos += "Modalidade, ";
                    }
                    if (dados.modelo == 0)
                    {
                        camposInvalidos += "Modelo, ";
                    }
                    if (dados.area == 0)
                    {
                        camposInvalidos += "Área, ";
                    }
                    JsonObject retornoVerificacao = new JsonObject();
                    retornoVerificacao.Add("CODIGO", 1);
                    retornoVerificacao.Add("MENSAGEM", camposInvalidos.Substring(0, (camposInvalidos.Length - 2)) + ".");
                    return retornoVerificacao;
                }
                EmpresaVaga vaga = new EmpresaVaga(dados.id, dados.titulo, dados.descricao, dados.modalidade, dados.modelo, dados.area, dados.dataLimite, dados.status);
                return EmpresaVaga.EditarVaga(vaga);
            };
            app.MapPost("/empresa/vaga/editar", editarEmpresaVaga);

            var excluirEmpresaVaga = (long id) => {
                EmpresaVaga vaga = new EmpresaVaga(id);
                return EmpresaVaga.ExcluirVaga(vaga);
            };
            app.MapGet("/empresa/vaga/excluir/{id}", excluirEmpresaVaga);

            var consultasDiversas = () => {
                return ExecucoesDiversas.ConsultaSelectVaga();
            };
            app.MapGet("/consultas/select/vaga", consultasDiversas);

            var consultaVagasIds = () => {
                return ExecucoesDiversas.ConsultaVagasIds();
            };
            app.MapGet("/consulta/vagas/ids", consultaVagasIds);

            var consultaVagaId = (long id) => {
                return ExecucoesDiversas.ConsultaVagaId(id);
            };
            app.MapGet("/consulta/vaga/{id}", consultaVagaId);

            var curtirVaga = (long candidato, long vaga) => {
                return ExecucoesDiversas.CurtirVaga(candidato, vaga);
            };
            app.MapGet("/vaga/curtir/{candidato}/{vaga}", curtirVaga);

            var consultaCurtidas = (long candidato, long vaga) => {
                return ExecucoesDiversas.ConsultaCurtidas(candidato, vaga);
            };
            app.MapGet("/consulta/curtidas/{candidato}/{vaga}", consultaCurtidas);

            app.Run();
        }
    }
}