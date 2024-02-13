using System.Collections.Generic;

public class StableDiffusionRequest
{
    public string sd_model_checkpoint;
    public string prompt;
    public string negative_prompt;
    public int batch_size;
    public int steps;
    public float cfg_scale;
    public bool enable_hr;
    public int hr_resize_x;
    public int hr_resize_y;
    public float hr_scale;
    public string hr_upscaler;
    public int hr_second_pass_steps;
    public float denoising_strength;
    public string sampler_index;

    public AlwaysonScripts alwayson_scripts;
}

public class AlwaysonScripts
{
    public ControlnetScript controlnet;
}

public class ControlnetScript
{
    public List<ControlnetArgs> args;
}

public class ControlnetArgs
{
    public string input_image;
    public string model;
    public int resize_mode;
    public int control_mode;
    public int weight;
}