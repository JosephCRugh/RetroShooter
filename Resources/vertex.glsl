﻿#version 430

layout(location = 0) in vec2 v_position;
layout(location = 1) in vec2 v_uv;

out vec2 f_uv;

void main() {

	gl_Position = vec4(v_position, 0.0, 1.0);
	
	f_uv = vec2(v_uv.x, 1.0 - v_uv.y);

}