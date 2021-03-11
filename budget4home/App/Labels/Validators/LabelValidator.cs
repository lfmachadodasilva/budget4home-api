﻿using System;
using System.Threading.Tasks;

namespace budget4home.App.Labels.Validators
{
    public interface ILabelValidator
    {
        bool Validate(string name);

        Task<bool> ValidateAsync(long labelId, long groupId);
    }

    public class LabelValidator : ILabelValidator
    {
        private readonly ILabelRepository _labelRepository;

        public LabelValidator(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public bool Validate(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("INVALID_LABEL_NAME");
            }

            return true;
        }

        public async Task<bool> ValidateAsync(long labelId, long groupId)
        {
            var label = await _labelRepository.GetByIdAsync(labelId, true);

            if (label == null)
            {
                throw new ArgumentException("INVALID_LABEL");
            }

            if (!label.GroupId.Equals(groupId))
            {
                throw new ArgumentException("INVALID_LABEL_GROUP");
            }

            return true;
        }
    }
}