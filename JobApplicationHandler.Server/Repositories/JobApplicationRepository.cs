﻿using JobApplicationHandler.Contracts.JobApplications;
using JobApplicationHandler.Server.Repositories.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationHandler.Server.Repositories;

public interface IJobApplicationRepository
{
    Task<JobApplication?> GetJobApplicationByIdAsync(String id);
    Task<JobApplication> CreateJobApplicationAsync(JobApplication jobApplication);
    Task<JobApplication?> UpdateJobApplicationAsync(JobApplication jobApplication);
    Task<bool> DeleteJobApplicationAsync(Guid id);
}


public class JobApplicationRepository(JobApplicationDbContext dbContext) : IJobApplicationRepository
{
    public async Task<JobApplication?> GetJobApplicationByIdAsync(String id)
    {
        return await dbContext.JobApplications
            .FirstOrDefaultAsync(ja => ja.Id == id);
    }

    public async Task<JobApplication> CreateJobApplicationAsync(JobApplication jobApplication)
    {
        dbContext.JobApplications.Add(jobApplication);
        await dbContext.SaveChangesAsync();
        return jobApplication;
    }

    public async Task<JobApplication?> UpdateJobApplicationAsync(JobApplication jobApplication)
    {
        var existing = await dbContext.JobApplications.FindAsync(jobApplication.Id);
        if (existing == null) return null;

        dbContext.Entry(existing).CurrentValues.SetValues(jobApplication);
        await dbContext.SaveChangesAsync();
        return existing;
    }

    
    public async Task<bool> DeleteJobApplicationAsync(Guid id)
    {
        var jobApp = await dbContext.JobApplications.FindAsync(id);
        if (jobApp == null) return false;

        dbContext.JobApplications.Remove(jobApp);
        await dbContext.SaveChangesAsync();
        return true;
    }
}