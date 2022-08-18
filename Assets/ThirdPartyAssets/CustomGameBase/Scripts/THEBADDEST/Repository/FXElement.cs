using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class FXElement : Element
{

	ParticleSystem particleSystem;
	public ParticleSystem Particle
	{
		get
		{
			if (particleSystem == null)
			{
				particleSystem = GetComponent<ParticleSystem>();
			}

			return particleSystem;
		}
	}

	public override void Simulate(bool value)
	{
		base.Simulate(value);
		if (value)
			Particle.Play();
		else
			Particle.Stop();
	}

	public void Simulate(Vector3 pos)
	{
		gameObject.SetActive(true);
		ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams {position = pos};
		Particle.Emit(emitParams, 1);
	}

	public void Simulate(Vector3 pos, Vector3 offset)
	{
		gameObject.SetActive(true);
		ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams {position = pos + offset};
		Particle.Emit(emitParams, 1);
	}

	public void Simulate(Vector3 pos, Quaternion rot)
	{
		gameObject.SetActive(true);
		ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams {position = pos, rotation3D = rot.eulerAngles};
		Particle.Emit(emitParams, 1);
	}

	// ParticleSystem.EmissionModule em = gasParticle.emission;
	// em.rateOverTime = ControlInput.handbrake > 0.5f ? 0 : Mathf.Lerp(em.rateOverTime.constant, Mathf.Clamp(150.0f * ControlInput.throttle, 30.0f, 100.0f), 0.1f);

}