using MySql.Data.MySqlClient;
using System.Globalization;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace LidaCarreiras.Classes
{
    public class CandidatoFormacao
    {
        private long Candidato { get; set; }
        private string? Instituicao { get; set; }
        private string? Curso { get; set; }
        private string? DataInicio { get; set; }
        private string? DataConclusao { get; set; }
        private string? DataCadastro { get; set; }

        public CandidatoFormacao(long candidato, string instituicao, string curso, string dataInicio, string dataConclusao, string dataCadastro)
        {
            this.Candidato = candidato;
            this.Instituicao = instituicao;
            this.Curso = curso;
            this.DataInicio = dataInicio;
            this.DataConclusao = dataConclusao;
            this.DataCadastro = dataCadastro;
        }

        public CandidatoFormacao(long candidato)
        {
            this.Candidato = candidato;
        }

        public CandidatoFormacao(long candidato, string dataCadastro)
        {
            this.Candidato = candidato;
            this.DataCadastro = dataCadastro;
        }

        public static JsonObject RegistrarFormacao(CandidatoFormacao formacao)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "INSERT INTO candidatos_formacoes (Candidato,Instituicao,Curso,DataInicio,DataConclusao,DataCadastro) VALUES (@Candidato,@Instituicao,@Curso,@DataInicio,@DataConclusao,@DataCadastro)";
                cmd.Parameters.AddWithValue("@Candidato", formacao.Candidato);
                cmd.Parameters.AddWithValue("@Instituicao", formacao.Instituicao);
                cmd.Parameters.AddWithValue("@Curso", formacao.Curso);
                cmd.Parameters.AddWithValue("@DataInicio", formacao.DataInicio);
                cmd.Parameters.AddWithValue("@DataConclusao", (formacao.DataConclusao != "") ? formacao.DataConclusao : null);
                cmd.Parameters.AddWithValue("@DataCadastro", formacao.DataCadastro);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Formação registrada com sucesso!");
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Não foi possível registrar no momento!");
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

        public static JsonObject EditarFormacao(CandidatoFormacao formacao)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "UPDATE candidatos_formacoes SET Instituicao = @Instituicao,Curso = @Curso,DataInicio = @DataInicio,DataConclusao = @DataConclusao WHERE Candidato = @Candidato AND DataCadastro = @DataCadastro";
                cmd.Parameters.AddWithValue("@Candidato", formacao.Candidato);
                cmd.Parameters.AddWithValue("@Instituicao", formacao.Instituicao);
                cmd.Parameters.AddWithValue("@Curso", formacao.Curso);
                cmd.Parameters.AddWithValue("@DataInicio", formacao.DataInicio);
                cmd.Parameters.AddWithValue("@DataConclusao", formacao.DataConclusao);
                cmd.Parameters.AddWithValue("@DataCadastro", formacao.DataCadastro);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Formação alterada com sucesso!");
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Não foi possível alterar no momento!");
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

        public static JsonObject ConsultarFormacao(CandidatoFormacao formacao)
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
                cmd.CommandText = "SELECT Instituicao, Curso, DataInicio, DataConclusao, DATE_FORMAT(DataCadastro, '%Y-%m-%d %H:%i:%s.%f') AS DataCadastro FROM candidatos_formacoes WHERE Candidato = @Candidato";
                cmd.Parameters.AddWithValue("@Candidato", formacao.Candidato);
                cmd.Prepare();
                var executa = cmd.ExecuteReader();
                JsonObject formacoes = new JsonObject();
                formacoes.Add("Candidato", formacao.Candidato);
                JsonArray formacaoArray = new JsonArray();
                if (executa.HasRows)
                {
                    while (executa.Read())
                    {
                        JsonObject formacaoTemp = new JsonObject();
                        formacaoTemp.Add("Instituicao", executa.GetString("Instituicao"));
                        formacaoTemp.Add("Curso", executa.GetString("Curso"));
                        formacaoTemp.Add("DataInicio", DateTime.ParseExact(executa.GetString("DataInicio"), "dd/MM/yyyy HH:mm:ss", culture).ToString("dd/MM/yyyy"));
                        formacaoTemp.Add("DataConclusao", (!executa.IsDBNull(executa.GetOrdinal("DataConclusao"))) ? DateTime.ParseExact(executa.GetString("DataConclusao"), "dd/MM/yyyy HH:mm:ss", culture).ToString("dd/MM/yyyy") : "-");
                        formacaoTemp.Add("DataCadastro", executa.GetString("DataCadastro"));
                        formacaoArray.Add(formacaoTemp);
                    }
                    formacoes.Add("Formacoes", formacaoArray);
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("CONTEUDO", formacoes);
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Nenhum registro foi encontrado!");
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

        public static JsonObject DeletarFormacao(CandidatoFormacao formacao)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "DELETE FROM candidatos_formacoes WHERE Candidato = @Candidato AND DataCadastro = @DataCadastro";
                cmd.Parameters.AddWithValue("@Candidato", formacao.Candidato);
                cmd.Parameters.AddWithValue("@DataCadastro", formacao.DataCadastro);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Formação deletada com sucesso!");
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Não foi possível deletar no momento!");
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

        public static JsonObject ExcluirFormacao(CandidatoFormacao formacao)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "DELETE FROM candidatos_formacoes WHERE Candidato = @Candidato AND DataCadastro = @DataCadastro";
                cmd.Parameters.AddWithValue("@Candidato", formacao.Candidato);
                cmd.Parameters.AddWithValue("@DataCadastro", formacao.DataCadastro);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Formação excluída com sucesso!");
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Não foi possível excluir no momento!");
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
