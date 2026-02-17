import { useEffect, useState } from "react";

// const API_URL = "http://localhost:5010/api/todos"; // Postgres
//const API_URL = "https://localhost:5159/api/todos"; // Mongo
const API_URL = "http://localhost:5001/api/todos"; // docker Postgres API

export default function App() {
  const [text, setText] = useState("");
  const [todos, setTodos] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  // Load todos from API
  useEffect(() => {
    loadTodos();
  }, []);

  const loadTodos = async () => {
    try {
      setLoading(true);
      const res = await fetch(API_URL);
      if (!res.ok) throw new Error("Failed to load todos");
      const data = await res.json();
      setTodos(data);
    } catch (err) {
      console.error(err);
      setError("API not reachable. Is backend running?");
    } finally {
      setLoading(false);
    }
  };

  const addTodo = async () => {
    if (!text.trim()) return;

    try {
      const res = await fetch(API_URL, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ text, done: false }),
      });

      if (!res.ok) throw new Error("Failed to add todo");
      const saved = await res.json();
      setTodos([...todos, saved]);
      setText("");
    } catch (err) {
      console.error(err);
      setError("Failed to add todo");
    }
  };

  const toggleDone = async (todo) => {
    try {
      const res = await fetch(`${API_URL}/${todo.id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ ...todo, done: !todo.done }),
      });

      if (!res.ok) throw new Error("Failed to update todo");

      setTodos(
        todos.map((t) => (t.id === todo.id ? { ...t, done: !t.done } : t)),
      );
    } catch (err) {
      console.error(err);
      setError("Failed to update todo");
    }
  };

  const deleteTodo = async (id) => {
    try {
      const res = await fetch(`${API_URL}/${id}`, { method: "DELETE" });
      if (!res.ok) throw new Error("Failed to delete todo");

      setTodos(todos.filter((t) => t.id !== id));
    } catch (err) {
      console.error(err);
      setError("Failed to delete todo");
    }
  };

  return (
    <div style={{ padding: 24, maxWidth: 500, margin: "0 auto" }}>
      <h2>Todo App (React + .NET + MongoDB)</h2>

      {error && <p style={{ color: "red", marginBottom: 10 }}>⚠️ {error}</p>}

      <div style={{ display: "flex", gap: 8 }}>
        <input
          value={text}
          onChange={(e) => setText(e.target.value)}
          placeholder="Enter todo..."
          style={{ flex: 1, padding: 8 }}
        />
        <button onClick={addTodo}>Add</button>
      </div>

      {loading && <p>Loading...</p>}

      <ul style={{ marginTop: 20 }}>
        {todos.map((t) => (
          <li
            key={t.id}
            style={{
              display: "flex",
              alignItems: "center",
              gap: 8,
              marginBottom: 6,
            }}
          >
            <input
              type="checkbox"
              checked={t.done}
              onChange={() => toggleDone(t)}
            />
            <span style={{ textDecoration: t.done ? "line-through" : "none" }}>
              {t.text}
            </span>
            <button onClick={() => deleteTodo(t.id)}>❌</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
