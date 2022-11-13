using MySql.Data.MySqlClient;
using System.Globalization;
using System.Text.Json.Nodes;

namespace LidaCarreiras.Classes
{
    public class CandidatoCurriculo
    {
        public static JsonObject ConsultarCurriculo(long id)
        {
            var culture = new CultureInfo("pt-BR", false);
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT * FROM candidatos WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Prepare();
                var executaBuscaCandidato = cmd.ExecuteReader();
                JsonObject candidato = new JsonObject();
                if (executaBuscaCandidato.HasRows)
                {
                    while (executaBuscaCandidato.Read())
                    {
                        candidato.Add("Id", long.Parse(executaBuscaCandidato.GetString("Id")));
                        candidato.Add("Nome", executaBuscaCandidato.GetString("Nome"));
                        candidato.Add("NomeSocial", (!executaBuscaCandidato.IsDBNull(executaBuscaCandidato.GetOrdinal("NomeSocial"))) ? executaBuscaCandidato.GetString("NomeSocial") : null);
                        candidato.Add("Sexo", executaBuscaCandidato.GetString("Sexo"));
                        candidato.Add("DataNascimento", DateTime.ParseExact(executaBuscaCandidato.GetString("DataNascimento"), "dd/MM/yyyy HH:mm:ss", culture).ToString("yyyy-MM-dd"));
                        candidato.Add("Sobre", (!executaBuscaCandidato.IsDBNull(executaBuscaCandidato.GetOrdinal("Sobre"))) ? executaBuscaCandidato.GetString("Sobre") : null);
                    }
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("CONTEUDO", candidato);
                }
                else
                {
                    cmd.Connection.Close();
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Nenhum registro foi encontrado!");
                    return retornoJson;
                }
                cmd.Connection.Close();
                conexao.Open();
                cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT * FROM enderecos WHERE Candidato = @Candidato";
                cmd.Parameters.AddWithValue("@Candidato", id);
                cmd.Prepare();
                var executaBuscaEndereco = cmd.ExecuteReader();
                JsonObject endereco = new JsonObject();
                if (executaBuscaEndereco.HasRows)
                {
                    while (executaBuscaEndereco.Read())
                    {
                        endereco.Add("Logradouro", executaBuscaEndereco.GetString("Logradouro"));
                        endereco.Add("Numero", executaBuscaEndereco.GetString("Numero"));
                        endereco.Add("Complemento", executaBuscaEndereco.GetString("Complemento"));
                        endereco.Add("Cep", executaBuscaEndereco.GetString("Cep"));
                        endereco.Add("Bairro", executaBuscaEndereco.GetString("Bairro"));
                        endereco.Add("Cidade", executaBuscaEndereco.GetString("Cidade"));
                        endereco.Add("Estado", executaBuscaEndereco.GetString("Estado"));
                    }
                    candidato.Add("Endereco", endereco);
                }
                else
                {
                    candidato.Add("Endereco", null);
                }
                cmd.Connection.Close();
                conexao.Open();
                cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT * FROM candidatos_conhecimentos WHERE Candidato = @Candidato ORDER BY Conhecimento ASC";
                cmd.Parameters.AddWithValue("@Candidato", id);
                cmd.Prepare();
                var executaBuscaConhecimento = cmd.ExecuteReader();
                JsonArray conhecimento = new JsonArray();
                if (executaBuscaConhecimento.HasRows)
                {
                    while (executaBuscaConhecimento.Read())
                    {
                        conhecimento.Add(executaBuscaConhecimento.GetString("Conhecimento"));
                    }
                    candidato.Add("Conhecimentos", conhecimento);
                }
                else
                {
                    candidato.Add("Conhecimentos", null);
                }
                cmd.Connection.Close();
                conexao.Open();
                cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT * FROM candidatos_experiencias WHERE Candidato = @Candidato ORDER BY DataContratacao ASC";
                cmd.Parameters.AddWithValue("@Candidato", id);
                cmd.Prepare();
                var executaBuscaExperiencia = cmd.ExecuteReader();
                JsonArray experiencia = new JsonArray();
                if (executaBuscaExperiencia.HasRows)
                {
                    while (executaBuscaExperiencia.Read())
                    {
                        JsonObject experienciaTemp = new JsonObject();
                        experienciaTemp.Add("Empresa", executaBuscaExperiencia.GetString("Empresa"));
                        experienciaTemp.Add("Funcao", executaBuscaExperiencia.GetString("Funcao"));
                        experienciaTemp.Add("Descricao", executaBuscaExperiencia.GetString("Descricao"));
                        experienciaTemp.Add("DataContratacao", DateTime.ParseExact(executaBuscaExperiencia.GetString("DataContratacao"), "dd/MM/yyyy HH:mm:ss", culture).ToString("yyyy-MM-dd"));
                        experienciaTemp.Add("DataDesligamento", (!executaBuscaExperiencia.IsDBNull(executaBuscaExperiencia.GetOrdinal("DataDesligamento"))) ? DateTime.ParseExact(executaBuscaExperiencia.GetString("DataDesligamento"), "dd/MM/yyyy HH:mm:ss", culture).ToString("yyyy-MM-dd") : null);
                        experiencia.Add(experienciaTemp);
                    }
                    candidato.Add("Experiencias", experiencia);
                }
                else
                {
                    candidato.Add("Experiencias", null);
                }
                cmd.Connection.Close();
                conexao.Open();
                cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT * FROM candidatos_formacoes WHERE Candidato = @Candidato ORDER BY DataInicio ASC";
                cmd.Parameters.AddWithValue("@Candidato", id);
                cmd.Prepare();
                var executaBuscaFormacao = cmd.ExecuteReader();
                JsonArray formacao = new JsonArray();
                if (executaBuscaFormacao.HasRows)
                {
                    while (executaBuscaFormacao.Read())
                    {
                        JsonObject formacaoTemp = new JsonObject();
                        formacaoTemp.Add("Instituicao", executaBuscaFormacao.GetString("Instituicao"));
                        formacaoTemp.Add("Curso", executaBuscaFormacao.GetString("Curso"));
                        formacaoTemp.Add("DataInicio", DateTime.ParseExact(executaBuscaFormacao.GetString("DataInicio"), "dd/MM/yyyy HH:mm:ss", culture).ToString("yyyy-MM-dd"));
                        formacaoTemp.Add("DataConclusao", (!executaBuscaFormacao.IsDBNull(executaBuscaFormacao.GetOrdinal("DataConclusao"))) ? DateTime.ParseExact(executaBuscaFormacao.GetString("DataConclusao"), "dd/MM/yyyy HH:mm:ss", culture).ToString("yyyy-MM-dd") : null);
                        formacao.Add(formacaoTemp);
                    }
                    candidato.Add("Formacoes", formacao);
                }
                else
                {
                    candidato.Add("Formacoes", null);
                }
                cmd.Connection.Close();
                conexao.Open();
                cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT * FROM candidatos_certificados WHERE Candidato = @Candidato ORDER BY DataCadastro ASC";
                cmd.Parameters.AddWithValue("@Candidato", id);
                cmd.Prepare();
                var executaBuscaCertificado = cmd.ExecuteReader();
                JsonArray certificado = new JsonArray();
                if (executaBuscaCertificado.HasRows)
                {
                    while (executaBuscaCertificado.Read())
                    {
                        JsonObject certificadoTemp = new JsonObject();
                        certificadoTemp.Add("Instituicao", executaBuscaCertificado.GetString("Instituicao"));
                        certificadoTemp.Add("Titulo", executaBuscaCertificado.GetString("Titulo"));
                        certificadoTemp.Add("Anexo", (!executaBuscaCertificado.IsDBNull(executaBuscaCertificado.GetOrdinal("Anexo"))) ? executaBuscaCertificado.GetString("Anexo") : null);
                        certificadoTemp.Add("Url", (!executaBuscaCertificado.IsDBNull(executaBuscaCertificado.GetOrdinal("Url"))) ? executaBuscaCertificado.GetString("Url") : null);
                        certificado.Add(certificadoTemp);
                    }
                    candidato.Add("Certificados", certificado);
                }
                else
                {
                    candidato.Add("Certificados", null);
                }
                cmd.Connection.Close();
            }
            catch (MySqlException e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            catch (Exception e)
            {
                retornoJson.Add("CODIGO", 2);
                retornoJson.Add("MENSAGEM", e.Message);
                return retornoJson;
            }
            return retornoJson;
        }
    }
}