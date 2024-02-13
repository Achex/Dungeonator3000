using System.Collections.Generic;

public class ControlnetRequest
{
    public string controlnet_module;
    public List<string> controlnet_input_images;
    public int controlnet_processor_res;
    public int controlnet_threshold_a;
    public int controlnet_threshold_b;
}
