using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FIXME : this will not serialize properly. will need to do some research into this, or replace in future with file system data

[System.Serializable]
public class serializable_dict<key_t, value_t>
{
	public key_value_pair<key_t, value_t>[] key_value_pairs;

	private Dictionary<key_t, value_t> dictionary = new Dictionary<key_t, value_t>();
}

[System.Serializable]
public class key_value_pair<key_t, value_t>
{
	key_t key;
	value_t value;
}
