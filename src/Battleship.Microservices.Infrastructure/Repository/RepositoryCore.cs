namespace Battleship.Microservices.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Dapper;
    using Microsoft.Data.SqlClient;

    public class RepositoryCore : IRepositoryCore
    {
        #region Fields

        private readonly string databaseName;
        private readonly int    timeout = 360;

        #endregion

        #region Constructors

        public RepositoryCore(string databaseName)
        {
            this.databaseName = databaseName;
        }

        #endregion

        #region Methods

        private SqlConnection GetOpenConnection()
        {
            var sqlConnection = new SqlConnection(this.databaseName);
            sqlConnection.Open();
            return sqlConnection;
        }

        /// <summary>
        ///     Executes the specified parameters and stored procedure vai dapper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <param name="procedure">The procedure name.</param>
        /// <returns>Type of T</returns>
        /// <exception cref="System.Exception">Execution failed.</exception>
        protected IEnumerable<T> Execute<T>(Dictionary<string, object> parameters,
            [CallerMemberName] string procedure = "") where T : class
        {
            IEnumerable<T> result = null;
            var connection = this.GetOpenConnection();
            try
            {
                using (connection)
                {
                    var dynamicParameters = new DynamicParameters();
                    if (parameters != null && parameters.Count > 0)
                        dynamicParameters = this.SetupDynamicParameters(parameters);
                    result = connection.Query<T>(this.SetName(procedure), dynamicParameters,
                                                 commandType: CommandType.StoredProcedure, commandTimeout: this.timeout);
                }
            }
            catch (SqlException exp)
            {
                throw new Exception(exp.Message, exp);
            }
            catch (Exception exp)
            {
                throw new Exception("Execution failed.", exp);
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        protected IEnumerable<T> Execute<T>([CallerMemberName] string procedure = "") where T : class
        {
            var connection = this.GetOpenConnection();
            try
            {
                using (connection)
                {
                    return connection.Query<T>(this.SetName(procedure), commandType: CommandType.StoredProcedure,
                                               commandTimeout: this.timeout);
                }
            }
            catch (SqlException exp)
            {
                throw new Exception(exp.Message, exp);
            }
            catch (Exception exp)
            {
                throw new Exception("Execution failed.", exp);
            }
            finally
            {
                connection.Close();
            }
        }


        /// <summary>
        ///     Executes the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="procedure">The procedure.</param>
        /// <exception cref="System.Exception">Execution failed.</exception>
        protected int Execute(Dictionary<string, object> parameters, [CallerMemberName] string procedure = "")
        {
            var connection = this.GetOpenConnection();
            try
            {
                using (connection)
                {
                    var dynamicParameters = new DynamicParameters();
                    if (parameters != null && parameters.Count > 0)
                        dynamicParameters = this.SetupDynamicParameters(parameters);
                    return connection.Execute(this.SetName(procedure), dynamicParameters,
                                              commandType: CommandType.StoredProcedure, commandTimeout: this.timeout);
                }
            }
            catch (SqlException exp)
            {
                throw new Exception(exp.Message, exp);
            }
            catch (Exception exp)
            {
                throw new Exception("Execution failed.", exp);
            }
            finally
            {
                connection.Close();
            }
        }

        protected async Task<IEnumerable<T>> ExecuteAsync<T>([CallerMemberName] string procedure = "") where T : class
        {
            var connection = this.GetOpenConnection();
            IEnumerable<T> result;
            try
            {
                using (connection)
                {
                    var sql = this.SetName(procedure);
                    CommandType? commandType = CommandType.StoredProcedure;
                    var commandTimeout = this.timeout;
                    result = await connection.QueryAsync<T>(sql, null, null, commandTimeout, commandType);
                }
            }
            catch (SqlException exp)
            {
                throw new Exception(exp.Message, exp);
            }
            catch (Exception exp)
            {
                throw new Exception("Execution failed.", exp);
            }
            finally
            {
                connection.Close();
            }

            return result;
        }


        protected async Task<int> ExecuteAsync(Dictionary<string, object> parameters,
            [CallerMemberName] string procedure = "")
        {
            var connection = this.GetOpenConnection();
            int result;
            try
            {
                using (connection)
                {
                    var dynamicParameters = new DynamicParameters();
                    if (parameters != null && parameters.Count > 0)
                        dynamicParameters = this.SetupDynamicParameters(parameters);

                    var sql = this.SetName(procedure);

                    var commandType = CommandType.StoredProcedure;
                    int? commandTimeout = this.timeout;
                    result = await connection.ExecuteAsync(sql, dynamicParameters, null, commandTimeout, commandType);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Execution failed.", ex);
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        /// <summary>
        ///     Executes and return single result.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <param name="procedure">The procedure.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Execution failed.</exception>
        protected T ExecuteScalar<T>(Dictionary<string, object> parameters,
            [CallerMemberName] string procedure = "")
        {
            var connection = this.GetOpenConnection();
            try
            {
                using (connection)
                {
                    var dynamicParameters = new DynamicParameters();
                    if (parameters != null && parameters.Count > 0)
                        dynamicParameters = this.SetupDynamicParameters(parameters);
                    return connection.Query<T>(this.SetName(procedure), dynamicParameters,
                                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (SqlException exp)
            {
                throw new Exception(exp.Message, exp);
            }
            catch (Exception exp)
            {
                throw new Exception("Execution failed.", exp);
            }
            finally
            {
                connection.Close();
            }
        }

        protected async Task<T> ExecuteScalarAsync<T>(Dictionary<string, object> parameters,
            [CallerMemberName] string procedure = "")
        {
            var connection = this.GetOpenConnection();
            T result;
            try
            {
                using (connection)
                {
                    var dynamicParameters = new DynamicParameters();
                    if (parameters != null && parameters.Count > 0)
                        dynamicParameters = this.SetupDynamicParameters(parameters);
                    result = (await connection.QueryAsync<T>(this.SetName(procedure), dynamicParameters, null, new int?(),
                                                             CommandType.StoredProcedure)).FirstOrDefault();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Execution failed.", ex);
            }
            finally
            {
                connection.Close();
            }

            return result;
        }


        private string SetName(string procedureName)
        {
            return $"sp{procedureName}";
        }

        private DynamicParameters SetupDynamicParameters(Dictionary<string, object> parameters)
        {
            DynamicParameters dynamicParameters = null;

            if (parameters != null && parameters.Count > 0)
            {
                dynamicParameters = new DynamicParameters();
                foreach (KeyValuePair<string, object> entry in parameters)
                {
                    var key = $"@{entry.Key}";
                    var value = entry.Value;

                    dynamicParameters.Add(key, value);
                }
            }

            return dynamicParameters;
        }

        #endregion
    }
}