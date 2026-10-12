
using Pharmacy.Domain.Entities;
namespace Pharmacy.Application.Services
{

    public class BatchService
    {
        private readonly List<Batch> _batches = new();
        private int _currentId = 0;
        //private readonly IIdGenerator _idGenerator;
        //  public BatchService(IIdGenerator idGenerator) => _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));

        //------ Main Methods ------
        /*  public void AddBatch(Batch batch)
          {
              ArgumentNullException.ThrowIfNull(batch);
              if (HasBatchWithBatchNumber(batch.Medicine, batch.BatchNumber))
              {
                  throw new ArgumentException("BatchNumber Already Exists.");
              }
              _batches.Add(batch);
          }*/

        public Batch AddBatch(
                        string batchNumber,
                        Medicine medicine,
                        Supplier supplier,
                        int quantity,
                        DateTime productionDate,
                        DateTime expiryDate,
                        decimal purchasePrice)
        {
            if (HasBatchWithBatchNumber(medicine, batchNumber))
            {
                throw new ArgumentException("BatchNumber Already Exists.");
            }

            var batch = new Batch(
                ++_currentId,
                batchNumber,
                medicine,
                supplier,
                quantity,
                productionDate,
                expiryDate,
                purchasePrice);

            _batches.Add(batch);

            return batch;
        }

        public Batch? GetBatchById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero.");
            }
            return _batches.FirstOrDefault(b => b.Id == id);
        }
        public int GetNextId()
        {
            int maxId = 0;
            foreach (Batch b in _batches)
            {
                if (b.Id > maxId) maxId = b.Id;
            }
            return maxId + 1;
        }


        public List<Batch> GetBatchByMedicine(Medicine medicine)
        {
            ArgumentNullException.ThrowIfNull(medicine);
            return _batches.Where(b => b.Medicine.Id == medicine.Id).ToList();
        }
        public Batch? FindBatchByBatchNumber(Medicine medicine, string batchNumber)
        {
            ArgumentNullException.ThrowIfNull(medicine);
            ValidateBatchNumber(batchNumber);
            return _batches.FirstOrDefault(b => b.BatchNumber == batchNumber && b.Medicine.Id == medicine.Id);
        }



        // ------ Helper Methods ------
        private bool HasBatchWithBatchNumber(Medicine medicine, string batchNumber)
        {
            ValidateBatchNumber(batchNumber);
            ArgumentNullException.ThrowIfNull(medicine);
            return _batches.Any(b => b.BatchNumber == batchNumber &&
                               b.Medicine.Id == medicine.Id);
        }

        private static void ValidateBatchNumber(string batchNumber)
        {
            if (string.IsNullOrWhiteSpace(batchNumber))
            {
                throw new ArgumentException("BatchNumber cannot be empty or null", nameof(batchNumber));
            }
        }
    }
}