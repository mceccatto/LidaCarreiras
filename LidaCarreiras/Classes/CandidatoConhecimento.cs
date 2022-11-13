using MySql.Data.MySqlClient;
using System.Text.Json.Nodes;

namespace LidaCarreiras.Classes
{
    public class CandidatoConhecimento
    {
        private long Candidato { get; set; }
        private JsonArray? Conhecimento { get; set; }

        public CandidatoConhecimento(long candidato, JsonArray conhecimento)
        {
            this.Candidato = candidato;
            this.Conhecimento = conhecimento;
        }

        public CandidatoConhecimento(long candidato)
        {
            this.Candidato = candidato;
        }

        public static JsonObject RegistrarConhecimento(CandidatoConhecimento conhecimento)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                if (conhecimento.Conhecimento != null)
                {
                    for (int i = 0; i < conhecimento.Conhecimento.Count; i++)
                    {
                        cmd.CommandText = "INSERT INTO candidatos_conhecimentos (Candidato,Conhecimento,DataCadastro) VALUES (@Candidato,@Conhecimento,@DataCadastro)";
                        cmd.Parameters.AddWithValue("@Candidato", conhecimento.Candidato);
                        cmd.Parameters.AddWithValue("@Conhecimento", conhecimento.Conhecimento[i]);
                        cmd.Parameters.AddWithValue("@DataCadastro", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Conhecimento registrado com sucesso!");
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Conhecimento inválido!");
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

        public static JsonObject EditarConhecimento(CandidatoConhecimento conhecimento)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "DELETE FROM candidatos_conhecimentos WHERE Candidato = @Candidato";
                cmd.Parameters.AddWithValue("@Candidato", conhecimento.Candidato);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                if (conhecimento.Conhecimento != null)
                {
                    for (int i = 0; i < conhecimento.Conhecimento.Count; i++)
                    {
                        cmd.CommandText = "INSERT INTO candidatos_conhecimentos (Candidato,Conhecimento,DataCadastro) VALUES (@Candidato,@Conhecimento,@DataCadastro)";
                        cmd.Parameters.AddWithValue("@Candidato", conhecimento.Candidato);
                        cmd.Parameters.AddWithValue("@Conhecimento", conhecimento.Conhecimento[i]);
                        cmd.Parameters.AddWithValue("@DataCadastro", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Conhecimento editado com sucesso!");
                }
                else
                {
                    retornoJson.Add("CODIGO", 1);
                    retornoJson.Add("MENSAGEM", "Conhecimento inválido!");
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

        public static bool ConsultarCadastroConhecimento(CandidatoConhecimento conhecimento)
        {
            bool retorno = false;
            string conexaoString = StringConexao.Conexao();
            MySqlConnection conexao = new MySqlConnection(conexaoString);
            conexao.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexao;
            cmd.CommandText = "SELECT * FROM candidatos_conhecimentos WHERE Candidato = @Candidato";
            cmd.Parameters.AddWithValue("@Candidato", conhecimento.Candidato);
            cmd.Prepare();
            var executa = cmd.ExecuteReader();
            if (executa.HasRows)
            {
                retorno = true;
            }
            cmd.Connection.Close();
            return retorno;
        }

        public static JsonObject ConsultarConhecimento(CandidatoConhecimento conhecimento)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT * FROM candidatos_conhecimentos WHERE Candidato = @Candidato";
                cmd.Parameters.AddWithValue("@Candidato", conhecimento.Candidato);
                cmd.Prepare();
                var executa = cmd.ExecuteReader();
                JsonObject conhecimentos = new JsonObject();
                conhecimentos.Add("Candidato", conhecimento.Candidato);
                JsonArray conhecimentoArray = new JsonArray();
                if (executa.HasRows)
                {
                    while (executa.Read())
                    {
                        JsonObject conhecimentoTemp = new JsonObject();
                        conhecimentoTemp.Add("Conhecimento", executa.GetString("Conhecimento"));
                        conhecimentoTemp.Add("DataCadastro", executa.GetString("DataCadastro"));
                        conhecimentoArray.Add(conhecimentoTemp);
                    }
                    conhecimentos.Add("Conhecimentos", conhecimentoArray);
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("CONTEUDO", conhecimentos);
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

        public static JsonObject ExcluirConhecimento(CandidatoConhecimento conhecimento)
        {
            JsonObject retornoJson = new JsonObject();
            string conexaoString = StringConexao.Conexao();
            try
            {
                MySqlConnection conexao = new MySqlConnection(conexaoString);
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "DELETE FROM candidatos_conhecimentos WHERE Candidato = @Candidato";
                cmd.Parameters.AddWithValue("@Candidato", conhecimento.Candidato);
                cmd.Prepare();
                var executa = cmd.ExecuteNonQuery();
                if (executa > 0)
                {
                    retornoJson.Add("CODIGO", 0);
                    retornoJson.Add("MENSAGEM", "Conhecimento(s) excluído(s) com sucesso!");
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
