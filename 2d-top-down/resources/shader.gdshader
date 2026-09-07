//Written in GDScript Right now
shader_type canvas_item;

uniform vec4 chroma_key : source_color = vec4(1.0, 0.0, 1.0, 1.0); 
uniform float similarity_threshold : hint_range(0.0, 1.0) = 0.3;  
uniform float smoothness : hint_range(0.0, 1.0) = 0.05;           

//Code to chroma key textures
void fragment() {
    vec4 tex_color = texture(TEXTURE, UV);
    
    vec3 k = vec3(0.299, 0.587, 0.114);
    float tex_y = dot(tex_color.rgb, k);
    float chroma_y = dot(chroma_key.rgb, k);
    
    vec2 tex_uv = vec2(tex_color.r - tex_y, tex_color.b - tex_y);
    vec2 chroma_uv = vec2(chroma_key.r - chroma_y, chroma_key.b - chroma_y);
    
    float color_dist = distance(tex_uv, chroma_uv);
    
    float alpha = smoothstep(similarity_threshold, similarity_threshold + smoothness, color_dist);
    
    COLOR = vec4(tex_color.rgb, tex_color.a * alpha);
}
