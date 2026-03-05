"use client";

import { useState, useEffect } from "react";
import type { Persistor } from "redux-persist";

interface RehydrateGateProps {
  persistor: Persistor;
  children: React.ReactNode;
  loading?: React.ReactNode;
}

/**
 * Client-only gate that waits for redux-persist rehydration before rendering children.
 * Defers setState so it never runs synchronously inside the persistor callback,
 * avoiding "Can't perform a React state update on a component that hasn't mounted yet".
 */
export function RehydrateGate({ persistor, children, loading = null }: RehydrateGateProps) {
  const [bootstrapped, setBootstrapped] = useState(false);

  useEffect(() => {
    let unsub: (() => void) | undefined;

    const handlePersistorState = () => {
      const { bootstrapped: done } = persistor.getState();
      if (done) {
        // Defer so setState is not triggered synchronously from the subscription callback
        queueMicrotask(() => setBootstrapped(true));
        unsub?.();
      }
    };

    unsub = persistor.subscribe(handlePersistorState);
    handlePersistorState();

    return () => {
      unsub?.();
    };
  }, [persistor]);

  if (!bootstrapped) return <>{loading}</>;
  return <>{children}</>;
}
