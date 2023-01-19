using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;
using XpressHRMS.Data.Enums;
using XpressHRMS.Data.IRepository;
using static XpressHRMS.Data.DTO.BanksDTO;

namespace XpressHRMS.Data.Repository
{
    public class BankRepository : IBankRepository

    {
        private readonly string _connectionString;
        private readonly ILogger<BankRepository> _logger;
        private readonly IConfiguration _configuration;
        public BankRepository(IConfiguration configuration, ILogger<BankRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateBank(CreateBankDTO bankDTO)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.INSERT);
                    param.Add("@BankName", bankDTO.BankName == null ? "" : bankDTO.BankName.ToString().Trim());
                    param.Add("@CbnCode", bankDTO.CbnCode == null ? "" : bankDTO.CbnCode.ToString().Trim());
                    dynamic response = await _dapper.ExecuteAsync("Sp_Bank", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateBankBranch(CreateBankDTO bankDTO) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateBank(UpdateBankDTO bankDTO)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.UPDATE);
                    param.Add("@BankID", Convert.ToInt32(bankDTO.BankID));
                    param.Add("@BankName", bankDTO.BankName == null ? "" : bankDTO.BankName.ToString().Trim());
                    param.Add("@CbnCode", bankDTO.CbnCode == null ? "" : bankDTO.CbnCode.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync("Sp_Bank", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateBank(UpdateBankDTO bankDTO) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteBank(DeleteBankDTO deleteBank)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@BankID", Convert.ToInt32(deleteBank.BankID));

                    dynamic response = await _dapper.ExecuteAsync("Sp_Bank", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteBanks(DeleteBankDTO deleteBank) ===>{ex.Message}");
                throw;
            }
        }


        public async Task<IEnumerable<BanksDTO>> GetAllBank()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.SELECTALL);
                    var bankbranch = await _dapper.QueryAsync<BanksDTO>("Sp_Bank", param: param, commandType: CommandType.StoredProcedure);

                    return bankbranch;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllBank() ===>{ ex.Message}");
                throw;
            }
        }

        public async Task<BanksDTO> GetBankById(double bankID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.SELECTBYID);
                    param.Add("@BankID", bankID);
                    var Bank = await _dapper.QueryFirstOrDefaultAsync<BanksDTO>("Sp_Bank", param: param, commandType: CommandType.StoredProcedure);

                    return Bank;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetBankById(int bankID) ===>{ ex.Message}");
                throw;
            }
        }


        public async Task<BanksDTO> GetBankByCBNCode(string BankCode)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 9);
                    param.Add("@CbnCode", BankCode.Trim());

                    var banks = await _dapper.QueryFirstOrDefaultAsync<BanksDTO>("Sp_Bank", param: param, commandType: CommandType.StoredProcedure);

                    return banks;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetBankByCBNCode(string BankCode) ===>{ ex.Message}");
                throw;
            }
        }
    }
}
