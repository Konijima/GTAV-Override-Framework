using System;
using GTA;
using GTA.UI;
using GTA.Native;

namespace GTAVOverride.Scripts
{
    class EconomyScript : Script
    {
        public static readonly int maxAmount = 999999999;
        public static readonly float maxInterest = 0.25f;

        private bool _hasBank;

        private int _cash;
        private int _bank;

        private int _totalGain;
        private int _totalSpent;

        private float _bankInterest;
        private DateTime _lastInterestDate;
        private TimeSpan _lastInterestTime;

        private bool _inited = false;

        public EconomyScript()
        {
            _hasBank = true;
            _cash = 0;
            _bank = 0;
            _totalGain = 0;
            _totalSpent = 0;
            _bankInterest = 0;
            _lastInterestDate = World.CurrentDate;
            _lastInterestTime = World.CurrentTimeOfDay;

            Tick += EconomyScript_Tick;
        }

        private void EconomyScript_Tick(object sender, EventArgs e)
        {
            if (!_inited)
            {
                Init();
            }
        }

        public void Init()
        {
            _inited = true;

            GainedCash += EconomyScript_GainedCash;
            SpentCash += EconomyScript_SpentCash;
            DepositedBank += EconomyScript_DepositedBank;
            WithdrewBank += EconomyScript_WithdrewBank;
            BankInterestReceived += EconomyScript_BankInterestReceived;
            BankAccountOpened += EconomyScript_BankAccountOpened;
            BankAccountClosed += EconomyScript_BankAccountClosed;
        }

        private void EconomyScript_GainedCash(object sender, EconomyEventArgs e)
        {
            
        }

        private void EconomyScript_SpentCash(object sender, EconomyEventArgs e)
        {
            
        }

        private void EconomyScript_DepositedBank(object sender, EconomyEventArgs e)
        {
            
        }

        private void EconomyScript_WithdrewBank(object sender, EconomyEventArgs e)
        {
            
        }

        private void EconomyScript_BankInterestReceived(object sender, EconomyEventArgs e)
        {
            
        }

        private void EconomyScript_BankAccountOpened(object sender, EventArgs e)
        {
            
        }

        private void EconomyScript_BankAccountClosed(object sender, EventArgs e)
        {
            
        }

        public bool hasBank
        {
            get { return _hasBank; }
        }

        public int cash
        {
            get { return _cash; }
            set
            {
                if (value < 0) value = 0;
                if (value > maxAmount) value = maxAmount;
                _cash = value;
            }
        }

        public int bank
        {
            get { return _bank; }
            set
            {
                if (value < 0) value = 0;
                if (value > maxAmount) value = maxAmount;
                _bank = value;
            }
        }

        public int totalGain
        {
            get { return _totalGain; }
            set
            {
                if (value < 0) value = 0;
                if (value > maxAmount) value = maxAmount;
                _totalGain = value;
            }
        }

        public int totalSpent
        {
            get { return _totalSpent; }
            set
            {
                if (value < 0) value = 0;
                if (value > maxAmount) value = maxAmount;
                _totalSpent = value;
            }
        }

        public float bankInterest {
            get { return _bankInterest; }
            set
            {
                if (value < 0) value = 0;
                if (value > maxInterest) value = maxInterest;
                _bankInterest = value;
            }
        }

        public bool HasEnoughtCash(int amount)
        {
            return (amount <= cash);
        }

        public bool HasEnoughtBank(int amount)
        {
            return (amount <= bank);
        }

        public bool HasEnoughtTotal(int amount)
        {
            return (amount <= cash + bank);
        }

        public int CalculateInterestGain()
        {
            return (int)Math.Round(bank * _bankInterest);
        }

        public void AddBankInterest()
        {
            if (hasBank)
            {
                int interestGain = CalculateInterestGain();
                if (interestGain > maxAmount) interestGain = maxAmount;
                if (interestGain > 0)
                {
                    int gain = interestGain;
                    if (bank + interestGain > maxAmount)
                    {
                        if (interestGain > bank) gain = interestGain - bank;
                        else gain = bank - interestGain;
                    }
                    bank += gain;

                    _lastInterestDate = World.CurrentDate;
                    _lastInterestTime = World.CurrentTimeOfDay;

                    EconomyEventArgs e = new EconomyEventArgs();
                    e.amount = gain;
                    OnBankInterestReceived(e);
                }
            }
        }

        public void GainCash(int amount)
        {
            if (amount < 0) amount = 0;
            if (amount > 0)
            {
                cash += amount;

                EconomyEventArgs e = new EconomyEventArgs();
                e.amount = amount;
                OnGainedCash(e);
            }
        }

        public void SpendCash(int amount)
        {
            if (amount <= cash)
            {
                if (amount > 0)
                {
                    cash -= amount;

                    EconomyEventArgs e = new EconomyEventArgs();
                    e.amount = amount;
                    OnSpentCash(e);
                }
            }
        }

        public void WithdrawBank(int amount)
        {
            if (hasBank)
            {
                if (amount <= bank)
                {
                    if (amount > 0)
                    {
                        cash += amount;
                        bank -= amount;

                        EconomyEventArgs e = new EconomyEventArgs();
                        e.amount = amount;
                        OnWithdrewBank(e);
                    }
                }
            }
        }

        public void DepositBank(int amount)
        {
            if (hasBank)
            {
                if (amount <= cash)
                {
                    if (amount > 0)
                    {
                        cash -= amount;
                        bank += amount;

                        EconomyEventArgs e = new EconomyEventArgs();
                        e.amount = amount;
                        OnDepositedBank(e);
                    }
                }
            }
        }

        public void OpenBankAccount()
        {
            if (!_hasBank)
            {
                _hasBank = true;

                _lastInterestDate = World.CurrentDate;
                _lastInterestTime = World.CurrentTimeOfDay;

                OnBankAccountOpened(new EventArgs());
            }
        }

        public void CloseBankAccount()
        {
            if (_hasBank)
            {
                if (bank == 0)
                {
                    _hasBank = false;

                    OnBankAccountClosed(new EventArgs());
                }
            }
        }

        protected virtual void OnGainedCash(EconomyEventArgs e)
        {
            GainedCash?.Invoke(this, e);
        }
        public event EventHandler<EconomyEventArgs> GainedCash;

        protected virtual void OnSpentCash(EconomyEventArgs e)
        {
            SpentCash?.Invoke(this, e);
        }
        public event EventHandler<EconomyEventArgs> SpentCash;

        protected virtual void OnWithdrewBank(EconomyEventArgs e)
        {
            WithdrewBank?.Invoke(this, e);
        }
        public event EventHandler<EconomyEventArgs> WithdrewBank;

        protected virtual void OnDepositedBank(EconomyEventArgs e)
        {
            DepositedBank?.Invoke(this, e);
        }
        public event EventHandler<EconomyEventArgs> DepositedBank;

        protected virtual void OnBankInterestReceived(EconomyEventArgs e)
        {
            BankInterestReceived?.Invoke(this, e);
        }
        public event EventHandler<EconomyEventArgs> BankInterestReceived;

        protected virtual void OnBankAccountOpened(EventArgs e)
        {
            BankAccountOpened?.Invoke(this, e);
        }
        public event EventHandler<EventArgs> BankAccountOpened;

        protected virtual void OnBankAccountClosed(EventArgs e)
        {
            BankAccountClosed?.Invoke(this, e);
        }
        public event EventHandler<EventArgs> BankAccountClosed;
    }

    public class EconomyEventArgs : EventArgs
    {
        public int amount { get; set; }
    }
}
