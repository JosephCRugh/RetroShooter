#version 430

in vec2 f_uv;

uniform sampler2D u_txr;

out vec4 o_color;

void main() {
	o_color = texture(u_txr, f_uv);
}